using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Core.UseCases;

namespace UTMarket.Application;

/// <summary>
/// ImplementaciÃ³n concreta del caso de uso para obtener un producto por su ID.
/// </summary>
public sealed class GetProductByIdUseCaseImpl(IProductRepository productRepository) : IGetProductByIdUseCase
{
    public async Task<Product?> ExecuteAsync(int productId, CancellationToken ct = default)
    {
        if (productId <= 0)
        {
            throw new ArgumentException("El ID del producto debe ser un entero positivo.", nameof(productId));
        }

        return await productRepository.GetByIdAsync(productId, ct);
    }
}
