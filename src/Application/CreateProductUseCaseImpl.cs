using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Core.UseCases;

namespace UTMarket.Application;

/// <summary>
/// ImplementaciÃ³n concreta del caso de uso para registrar un nuevo producto.
/// </summary>
public sealed class CreateProductUseCaseImpl(IProductRepository productRepository) : ICreateProductUseCase
{
    public async Task<Product> ExecuteAsync(Product product, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(product);

        // LÃ³gica de validaciÃ³n bÃ¡sica a nivel de aplicaciÃ³n
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException("El nombre del producto es obligatorio.");
        }

        if (product.Price < 0)
        {
            throw new ArgumentException("El precio no puede ser negativo.");
        }

        return await productRepository.AddAsync(product, ct);
    }
}
