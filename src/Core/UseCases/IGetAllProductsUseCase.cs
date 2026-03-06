using UTMarket.Core.Entities;

namespace UTMarket.Core.UseCases;

/// <summary>
/// Caso de uso para obtener todos los productos del catÃ¡logo.
/// </summary>
public interface IGetAllProductsUseCase
{
    /// <summary>
    /// Recupera todos los productos de forma asÃ­ncrona y en streaming.
    /// </summary>
    /// <param name="ct">Token de cancelaciÃ³n.</param>
    /// <returns>Flujo asÃ­ncrono de objetos Product.</returns>
    IAsyncEnumerable<Product> ExecuteAsync(CancellationToken ct = default);
}
