using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Core.UseCases;

namespace UTMarket.Application;

/// <summary>
/// ImplementaciÃ³n concreta del caso de uso para bÃºsqueda dinÃ¡mica de productos.
/// </summary>
public sealed class FindProductsUseCaseImpl(IProductRepository productRepository) : IFindProductsUseCase
{
    public IAsyncEnumerable<Product> ExecuteAsync(ProductFilter filter, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(filter);
        return productRepository.FindAsync(filter, ct);
    }
}
