using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using UTMarket.Core.Entities;

namespace UTMarket.Core.Repositories;

/// <summary>
/// Contrato de repositorio de dominio puro para la entidad Product.
/// DiseÃ±ado para Native AOT y streaming de datos de alto rendimiento.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Obtiene todos los productos en modo streaming para minimizar el consumo de memoria.
    /// </summary>
    /// <param name="ct">Token de cancelaciÃ³n para la operaciÃ³n de I/O.</param>
    /// <returns>Flujo asÃ­ncrono de objetos de dominio Product.</returns>
    IAsyncEnumerable<Product> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Busca un producto por su identificador Ãºnico de persistencia.
    /// </summary>
    /// <param name="id">ID de la base de datos.</param>
    /// <param name="ct">Token de cancelaciÃ³n.</param>
    /// <param name="transaction">Optional database transaction.</param>
    /// <returns>Producto si se encuentra, de lo contrario null.</returns>
    Task<Product?> GetByIdAsync(int id, CancellationToken ct = default, IDbTransaction? transaction = null);

    /// <summary>
    /// Busca productos basándose en criterios de filtrado específicos (AOT-Safe).
    /// </summary>
    /// <param name="filter">Objeto de criterios de búsqueda.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <param name="transaction">Optional database transaction.</param>
    /// <returns>Lista asíncrona de productos que coinciden con el filtro.</returns>
    IAsyncEnumerable<Product> FindAsync(ProductFilter filter, CancellationToken ct = default, IDbTransaction? transaction = null);

    /// <summary>
    /// Registra un nuevo producto en la persistencia.
    /// </summary>
    /// <param name="product">Objeto de dominio completo.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <param name="transaction">Optional database transaction.</param>
    /// <returns>El producto registrado con su ID generado.</returns>
    Task<Product> AddAsync(Product product, CancellationToken ct = default, IDbTransaction? transaction = null);

    /// <summary>
    /// Actualiza la información completa de un producto existente.
    /// </summary>
    /// <param name="product">Objeto de dominio with the changes.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <param name="transaction">Optional database transaction.</param>
    Task UpdateAsync(Product product, CancellationToken ct = default, IDbTransaction? transaction = null);

    /// <summary>
    /// Realiza una actualización parcial y atómica del stock de un producto.
    /// </summary>
    /// <param name="productId">ID del producto.</param>
    /// <param name="newStock">Nueva cantidad en inventario.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <param name="transaction">Optional database transaction.</param>
    Task UpdateStockAsync(int productId, int newStock, CancellationToken ct = default, IDbTransaction? transaction = null);

    /// <summary>
    /// Elimina un producto de la persistencia (Baja lógica o física según política).
    /// </summary>
    /// <param name="productId">ID del producto.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <param name="transaction">Optional database transaction.</param>
    Task DeleteAsync(int productId, CancellationToken ct = default, IDbTransaction? transaction = null);
}
