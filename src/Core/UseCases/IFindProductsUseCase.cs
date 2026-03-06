using UTMarket.Core.Entities;

namespace UTMarket.Core.UseCases;

/// <summary>
/// Caso de uso para bÃºsqueda dinÃ¡mica de productos con filtros.
/// </summary>
public interface IFindProductsUseCase
{
    /// <summary>
    /// Recupera productos que coinciden con los criterios de filtrado.
    /// </summary>
    /// <param name="filter">Objeto con los criterios de bÃºsqueda.</param>
    /// <param name="ct">Token de cancelaciÃ³n.</param>
    /// <returns>Flujo asÃ­ncrono de productos filtrados.</returns>
    IAsyncEnumerable<Product> ExecuteAsync(ProductFilter filter, CancellationToken ct = default);
}
