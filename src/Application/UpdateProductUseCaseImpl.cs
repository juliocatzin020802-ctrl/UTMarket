using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Core.UseCases;

namespace UTMarket.Application;

/// <summary>
/// ImplementaciÃ³n concreta del caso de uso para actualizar un producto.
/// </summary>
public sealed class UpdateProductUseCaseImpl(IProductRepository productRepository) : IUpdateProductUseCase
{
    public async Task ExecuteAsync(Product product, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(product);

        // Verificar existencia antes de actualizar (LÃ³gica de negocio)
        var existing = await productRepository.GetByIdAsync(product.ProductID, ct);
        if (existing is null)
        {
            throw new KeyNotFoundException($"El producto con ID {product.ProductID} no existe.");
        }

        await productRepository.UpdateAsync(product, ct);
    }
}
