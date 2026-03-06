using UTMarket.Core.Repositories;
using UTMarket.Core.UseCases;

namespace UTMarket.Application;

/// <summary>
/// ImplementaciÃ³n concreta para la actualizaciÃ³n atÃ³mica de stock.
/// </summary>
public sealed class UpdateProductStockUseCaseImpl(IProductRepository productRepository) : IUpdateProductStockUseCase
{
    public async Task ExecuteAsync(int productId, int newStock, CancellationToken ct = default)
    {
        if (newStock < 0)
        {
            throw new ArgumentException("El stock no puede ser negativo.", nameof(newStock));
        }

        var existing = await productRepository.GetByIdAsync(productId, ct);
        if (existing is null)
        {
            throw new KeyNotFoundException($"El producto con ID {productId} no existe.");
        }

        await productRepository.UpdateStockAsync(productId, newStock, ct);
    }
}
