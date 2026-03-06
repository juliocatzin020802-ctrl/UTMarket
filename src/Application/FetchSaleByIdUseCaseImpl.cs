using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Core.UseCases;

namespace UTMarket.Application;

/// <summary>
/// Concretized use case implementation for fetching a sale by ID.
/// </summary>
public sealed class FetchSaleByIdUseCaseImpl(ISaleRepository saleRepository) : IFetchSaleByIdUseCase
{
    public async Task<Sale?> ExecuteAsync(int id, CancellationToken ct = default)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Sale ID must be a positive integer.", nameof(id));
        }

        return await saleRepository.GetByIdAsync(id, ct);
    }
}
