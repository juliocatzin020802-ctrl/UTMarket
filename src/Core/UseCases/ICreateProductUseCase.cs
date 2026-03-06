using UTMarket.Core.Entities;

namespace UTMarket.Core.UseCases;

/// <summary>
/// Caso de uso para registrar un nuevo producto.
/// </summary>
public interface ICreateProductUseCase
{
    /// <summary>
    /// Registra el producto en el sistema.
    /// </summary>
    /// <param name="product">Objeto de dominio Product.</param>
    /// <param name="ct">Token de cancelaciÃ³n.</param>
    /// <returns>El producto creado, usualmente con el ID asignado.</returns>
    Task<Product> ExecuteAsync(Product product, CancellationToken ct = default);
}
