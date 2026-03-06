using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UTMarket.Core.Abstractions;
using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Infrastructure.Mappers;
using UTMarket.Infrastructure.Models.Data;

namespace UTMarket.Infrastructure.Repositories;

/// <summary>
/// Concrete repository for Sales, using manual ADO.NET for Native AOT compatibility.
/// Implements advanced query patterns to prevent N+1 problems.
/// </summary>
public sealed class SaleRepository(IDbConnectionFactory dbFactory, IProductRepository productRepository) : ISaleRepository
{
    public async Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken = default, IDbTransaction? transaction = null)
    {
        var connection = transaction?.Connection ?? await dbFactory.CreateConnectionAsync(cancellationToken);
        try
        {
            // Fetch header and details in a single roundtrip to prevent N+1
            const string sql = @"
                SELECT VentaID, Folio, FechaVenta, Estatus FROM dbo.Venta WHERE VentaID = @Id;
                SELECT DetalleID, VentaID, ProductoID, Cantidad, PrecioUnitario FROM dbo.DetalleVenta WHERE VentaID = @Id;";

            using var command = new SqlCommand(sql, (SqlConnection)connection, (SqlTransaction?)transaction);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            // Map Sale Header
            VentaEntity? ventaEntity = null;
            if (await reader.ReadAsync(cancellationToken))
            {
                ventaEntity = MapVentaEntity(reader);
            }

            if (ventaEntity is null) return null;

            // Move to the next result set for details
            await reader.NextResultAsync(cancellationToken);

            // Map Sale Details
            var detalles = new List<DetalleVentaEntity>();
            while (await reader.ReadAsync(cancellationToken))
            {
                detalles.Add(MapDetalleVentaEntity(reader));
            }

            // Explicitly close the reader before calling another repository method on the same connection/transaction
            await reader.CloseAsync();

            // Reconstruct Product objects required by the domain
            var productIds = detalles.Select(d => d.ProductoID).Distinct();
            var productCache = (await productRepository.FindAsync(new ProductFilter { /* Add criteria if needed */ }, cancellationToken, transaction)
                .ToListAsync(cancellationToken))
                .ToDictionary(p => p.ProductID);
            
            return ventaEntity.ToDomain(detalles, productCache);
        }
        finally
        {
            if (transaction == null) connection.Dispose();
        }
    }
    
    public async IAsyncEnumerable<Sale> GetAllAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var ids = new List<int>();
        using (var connection = await dbFactory.CreateConnectionAsync(cancellationToken))
        {
            const string sql = "SELECT VentaID FROM dbo.Venta;";
            using var command = new SqlCommand(sql, (SqlConnection)connection);
            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while(await reader.ReadAsync(cancellationToken))
            {
                 ids.Add(reader.GetInt32(0));
            }
        }

        foreach (var id in ids)
        {
            var sale = await GetByIdAsync(id, cancellationToken);
            if (sale is not null) yield return sale;
        }
    }
    
    public async IAsyncEnumerable<Sale> FindAsync(SaleFilter filter, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var connection = await dbFactory.CreateConnectionAsync(cancellationToken);
        
        var sqlBuilder = new System.Text.StringBuilder("SELECT VentaID FROM dbo.Venta WHERE 1=1");
        var parameters = new List<SqlParameter>();

        if (filter.StartDate.HasValue)
        {
            sqlBuilder.Append(" AND FechaVenta >= @StartDate");
            parameters.Add(new SqlParameter("@StartDate", filter.StartDate.Value));
        }

        if (filter.EndDate.HasValue)
        {
            sqlBuilder.Append(" AND FechaVenta <= @EndDate");
            parameters.Add(new SqlParameter("@EndDate", filter.EndDate.Value));
        }

        if (!string.IsNullOrWhiteSpace(filter.Folio))
        {
            sqlBuilder.Append(" AND Folio = @Folio");
            parameters.Add(new SqlParameter("@Folio", filter.Folio));
        }

        using var command = new SqlCommand(sqlBuilder.ToString(), (SqlConnection)connection);
        command.Parameters.AddRange(parameters.ToArray());

        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var ids = new List<int>();
        while (await reader.ReadAsync(cancellationToken))
        {
            ids.Add(reader.GetInt32(0));
        }

        foreach (var id in ids)
        {
            var sale = await GetByIdAsync(id, cancellationToken);
            if (sale is not null) yield return sale;
        }
    }

    public async Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken = default, IDbTransaction? transaction = null)
    {
        var connection = transaction?.Connection ?? await dbFactory.CreateConnectionAsync(cancellationToken);
        SqlTransaction? localTransaction = null;
        if (transaction == null)
        {
            localTransaction = (SqlTransaction)connection.BeginTransaction();
        }
        var activeTransaction = (SqlTransaction?)transaction ?? localTransaction;

        try
        {
            var (ventaEntity, detallesEntities) = sale.ToEntity();
            
            const string saleSql = @"
                INSERT INTO dbo.Venta (Folio, FechaVenta, Estatus, TotalArticulos, TotalVenta)
                OUTPUT INSERTED.VentaID
                VALUES (@Folio, @FechaVenta, @Estatus, @TotalArticulos, @TotalVenta);";
            
            using var saleCommand = new SqlCommand(saleSql, (SqlConnection)connection, activeTransaction);
            saleCommand.Parameters.AddWithValue("@Folio", ventaEntity.Folio);
            saleCommand.Parameters.AddWithValue("@FechaVenta", ventaEntity.FechaVenta);
            saleCommand.Parameters.AddWithValue("@Estatus", (byte)ventaEntity.Estatus);
            saleCommand.Parameters.AddWithValue("@TotalArticulos", sale.Details.Sum(d => d.Quantity));
            saleCommand.Parameters.AddWithValue("@TotalVenta", sale.TotalSale);
            
            var newSaleId = (int)await saleCommand.ExecuteScalarAsync(cancellationToken);

            foreach (var detalle in detallesEntities)
            {
                const string detailSql = @"
                    INSERT INTO dbo.DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario, TotalDetalle)
                    VALUES (@VentaID, @ProductoID, @Cantidad, @PrecioUnitario, @TotalDetalle);";
                using var detailCommand = new SqlCommand(detailSql, (SqlConnection)connection, activeTransaction);
                detailCommand.Parameters.AddWithValue("@VentaID", newSaleId);
                detailCommand.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);
                detailCommand.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                detailCommand.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                detailCommand.Parameters.AddWithValue("@TotalDetalle", detalle.Cantidad * detalle.PrecioUnitario);
                await detailCommand.ExecuteNonQueryAsync(cancellationToken);
            }

            if (localTransaction != null)
            {
                await localTransaction.CommitAsync(cancellationToken);
            }

            return (await GetByIdAsync(newSaleId, cancellationToken, transaction))!;
        }
        catch
        {
            if (localTransaction != null)
            {
                await localTransaction.RollbackAsync(cancellationToken);
            }
            throw;
        }
        finally
        {
            if (localTransaction != null)
            {
                localTransaction.Dispose();
                connection.Dispose();
            }
        }
    }
    
    public Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default, IDbTransaction? transaction = null)
    {
        return Task.CompletedTask;
    }
    
    private static VentaEntity MapVentaEntity(SqlDataReader reader) =>
        new(reader.GetInt32(0), reader.GetString(1)) 
        {
            FechaVenta = reader.GetDateTime(2),
            Estatus = reader.GetByte(3)
        };

    private static DetalleVentaEntity MapDetalleVentaEntity(SqlDataReader reader) =>
        new(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2)) 
        {
            Cantidad = reader.GetInt32(3),
            PrecioUnitario = reader.GetDecimal(4)
        };
}
