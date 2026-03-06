using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Core.UseCases;

namespace UTMarket.Application;

/// <summary>
/// Concretized use case implementation for sale creation orchestration.
/// </summary>
public sealed class CreateSaleUseCaseImpl(
    ISaleRepository saleRepository,
    IProductRepository productRepository) : ICreateSaleUseCase
{
    public async Task<Sale> ExecuteAsync(Sale sale, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(sale);

        // Core Domain logic orchestration
        if (sale.Details is null || !sale.Details.Any())
        {
            throw new InvalidOperationException("A sale must contain at least one detail.");
        }

        // Validate product existence and potentially stock (simplification)
        foreach (var detail in sale.Details)
        {
            var product = await productRepository.GetByIdAsync(detail.Product.ProductID, ct);
            if (product is null)
            {
                throw new KeyNotFoundException($"Product with ID {detail.Product.ProductID} does not exist.");
            }
        }

        return await saleRepository.AddAsync(sale, ct);
    }
}
