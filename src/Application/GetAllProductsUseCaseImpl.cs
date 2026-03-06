using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Core.UseCases;

namespace UTMarket.Application;

/// <summary>
/// ImplementaciÃ³n concreta del caso de uso para obtener todos los productos.
/// </summary>
public sealed class GetAllProductsUseCaseImpl(IProductRepository productRepository) : IGetAllProductsUseCase
{
    public IAsyncEnumerable<Product> ExecuteAsync(CancellationToken ct = default)
    {
        return productRepository.GetAllAsync(ct);
    }
}
