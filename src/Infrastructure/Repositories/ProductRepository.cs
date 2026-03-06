using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
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
/// Concrete repository implementation for Products, optimized for Native AOT.
/// Uses manual mapping via SqlDataReader to avoid reflection.
/// </summary>
public sealed class ProductRepository(IDbConnectionFactory dbFactory) : IProductRepository
{
    public async IAsyncEnumerable<Product> GetAllAsync([EnumeratorCancellation] CancellationToken ct = default)
    {
        using var connection = await dbFactory.CreateConnectionAsync(ct);
        const string sql = "SELECT ProductoID, Nombre, SKU, Marca, Precio, Stock FROM dbo.Producto;";
        
        using var command = new SqlCommand(sql, (SqlConnection)connection);
        
        using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult, ct);
        
        while (await reader.ReadAsync(ct))
        {
            var entity = MapToEntity(reader);
            yield return entity.ToDomain();
        }
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken ct = default, IDbTransaction? transaction = null)
    {
        var connection = transaction?.Connection ?? await dbFactory.CreateConnectionAsync(ct);
        try
        {
            const string sql = "SELECT ProductoID, Nombre, SKU, Marca, Precio, Stock FROM dbo.Producto WHERE ProductoID = @Id;";

            using var command = new SqlCommand(sql, (SqlConnection)connection, (SqlTransaction?)transaction);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, ct);
            if (await reader.ReadAsync(ct))
            {
                var entity = MapToEntity(reader);
                return entity.ToDomain();
            }

            return null;
        }
        finally
        {
            if (transaction == null) connection.Dispose();
        }
    }
    
    public async IAsyncEnumerable<Product> FindAsync(ProductFilter filter, [EnumeratorCancellation] CancellationToken ct = default, IDbTransaction? transaction = null)
    {
        var connection = transaction?.Connection ?? await dbFactory.CreateConnectionAsync(ct);
        try
        {
            var (sql, parameters) = BuildFilterQuery(filter);

            using var command = new SqlCommand(sql, (SqlConnection)connection, (SqlTransaction?)transaction);
            foreach (var param in parameters)
            {
                command.Parameters.Add(param);
            }

            using var reader = await command.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                yield return MapToEntity(reader).ToDomain();
            }
        }
        finally
        {
            if (transaction == null) connection.Dispose();
        }
    }

    public async Task<Product> AddAsync(Product product, CancellationToken ct = default, IDbTransaction? transaction = null)
    {
        var connection = transaction?.Connection ?? await dbFactory.CreateConnectionAsync(ct);
        try
        {
            const string sql = @"
                INSERT INTO dbo.Producto (Nombre, SKU, Marca, Precio, Stock) 
                OUTPUT INSERTED.ProductoID
                VALUES (@Nombre, @SKU, @Marca, @Precio, @Stock);";
            
            var entity = product.ToEntity();
            using var command = new SqlCommand(sql, (SqlConnection)connection, (SqlTransaction?)transaction);
            command.Parameters.AddWithValue("@Nombre", entity.Nombre);
            command.Parameters.AddWithValue("@SKU", entity.SKU);
            command.Parameters.AddWithValue("@Marca", entity.Marca);
            command.Parameters.AddWithValue("@Precio", entity.Precio);
            command.Parameters.AddWithValue("@Stock", entity.Stock);

            var newId = (int)await command.ExecuteScalarAsync(ct);
            return (await GetByIdAsync(newId, ct, transaction))!;
        }
        finally
        {
            if (transaction == null) connection.Dispose();
        }
    }
    
    public async Task UpdateAsync(Product product, CancellationToken ct = default, IDbTransaction? transaction = null)
    {
        var connection = transaction?.Connection ?? await dbFactory.CreateConnectionAsync(ct);
        try
        {
            const string sql = @"
                UPDATE dbo.Producto SET 
                    Nombre = @Nombre, SKU = @SKU, Marca = @Marca, Precio = @Precio, Stock = @Stock 
                WHERE ProductoID = @ProductoID;";

            var entity = product.ToEntity();
            using var command = new SqlCommand(sql, (SqlConnection)connection, (SqlTransaction?)transaction);
            command.Parameters.AddWithValue("@ProductoID", entity.ProductoID);
            command.Parameters.AddWithValue("@Nombre", entity.Nombre);
            command.Parameters.AddWithValue("@SKU", entity.SKU);
            command.Parameters.AddWithValue("@Marca", entity.Marca);
            command.Parameters.AddWithValue("@Precio", entity.Precio);
            command.Parameters.AddWithValue("@Stock", entity.Stock);

            await command.ExecuteNonQueryAsync(ct);
        }
        finally
        {
            if (transaction == null) connection.Dispose();
        }
    }
    
    public async Task UpdateStockAsync(int productId, int newStock, CancellationToken ct = default, IDbTransaction? transaction = null)
    {
        var connection = transaction?.Connection ?? await dbFactory.CreateConnectionAsync(ct);
        try
        {
            const string sql = "UPDATE dbo.Producto SET Stock = @Stock WHERE ProductoID = @ProductoID;";
            
            using var command = new SqlCommand(sql, (SqlConnection)connection, (SqlTransaction?)transaction);
            command.Parameters.AddWithValue("@Stock", newStock);
            command.Parameters.AddWithValue("@ProductoID", productId);
            
            await command.ExecuteNonQueryAsync(ct);
        }
        finally
        {
            if (transaction == null) connection.Dispose();
        }
    }

    public async Task DeleteAsync(int productId, CancellationToken ct = default, IDbTransaction? transaction = null)
    {
        var connection = transaction?.Connection ?? await dbFactory.CreateConnectionAsync(ct);
        try
        {
            const string sql = "DELETE FROM dbo.Producto WHERE ProductoID = @ProductoID;";
            
            using var command = new SqlCommand(sql, (SqlConnection)connection, (SqlTransaction?)transaction);
            command.Parameters.AddWithValue("@ProductoID", productId);
            
            await command.ExecuteNonQueryAsync(ct);
        }
        finally
        {
            if (transaction == null) connection.Dispose();
        }
    }

    private static ProductoEntity MapToEntity(SqlDataReader reader)
    {
        // Manual mapping from reader to entity with NULL handling for Marca
        var entity = new ProductoEntity(
            reader.GetInt32(reader.GetOrdinal("ProductoID")), 
            reader.GetString(reader.GetOrdinal("SKU"))
        )
        {
            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
            Marca = reader.IsDBNull(reader.GetOrdinal("Marca")) ? string.Empty : reader.GetString(reader.GetOrdinal("Marca")),
            Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
            Stock = reader.GetInt32(reader.GetOrdinal("Stock"))
        };
        return entity;
    }

    private static (string, List<SqlParameter>) BuildFilterQuery(ProductFilter filter)
    {
        var sqlBuilder = new StringBuilder("SELECT ProductoID, Nombre, SKU, Marca, Precio, Stock FROM dbo.Producto WHERE 1=1");
        var parameters = new List<SqlParameter>();

        if (!string.IsNullOrWhiteSpace(filter.NameContains))
        {
            sqlBuilder.Append(" AND Nombre LIKE @Name");
            parameters.Add(new SqlParameter("@Name", $"%{filter.NameContains}%"));
        }
        if (!string.IsNullOrWhiteSpace(filter.Brand))
        {
            sqlBuilder.Append(" AND Marca = @Brand");
            parameters.Add(new SqlParameter("@Brand", filter.Brand));
        }
        // ... more filters here ...

        return (sqlBuilder.ToString(), parameters);
    }
}
