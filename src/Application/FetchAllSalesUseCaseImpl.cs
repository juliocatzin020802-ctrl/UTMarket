using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Core.UseCases;

namespace UTMarket.Application;

/// <summary>
/// Concretized use case implementation for fetching all sales.
/// </summary>
public sealed class FetchAllSalesUseCaseImpl(ISaleRepository saleRepository) : IFetchAllSalesUseCase
{
    public IAsyncEnumerable<Sale> ExecuteAsync(CancellationToken ct = default)
    {
        return saleRepository.GetAllAsync(ct);
    }
}
