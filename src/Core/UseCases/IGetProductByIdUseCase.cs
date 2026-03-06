using UTMarket.Core.Entities;

namespace UTMarket.Core.UseCases;

/// <summary>
/// Caso de uso para obtener un producto especÃ­fico por su ID.
/// </summary>
public interface IGetProductByIdUseCase
{
    /// <summary>
    /// Busca un producto por su identificador.
    /// </summary>
    /// <param name="productId">ID del producto.</param>
    /// <param name="ct">Token de cancelaciÃ³n.</param>
    /// <returns>El producto si se encuentra, de lo contrario null.</returns>
    Task<Product?> ExecuteAsync(int productId, CancellationToken ct = default);
}
