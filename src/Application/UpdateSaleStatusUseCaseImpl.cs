using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Core.UseCases;

namespace UTMarket.Application;

/// <summary>
/// Concretized use case implementation for updating a sale's status.
/// </summary>
public sealed class UpdateSaleStatusUseCaseImpl(ISaleRepository saleRepository) : IUpdateSaleStatusUseCase
{
    public async Task ExecuteAsync(int id, SaleStatus newStatus, CancellationToken ct = default)
    {
        var existing = await saleRepository.GetByIdAsync(id, ct);
        if (existing is null)
        {
            throw new KeyNotFoundException($"Sale with ID {id} not found.");
        }

        // Logic check: only allowed status transitions could go here
        
        // As recommended previously, partial update logic
        // For now, since ISaleRepository only has UpdateAsync, we would have to load, update, and save
        existing.Status = newStatus;
        await saleRepository.UpdateAsync(existing, ct);
    }
}
