using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Core.UseCases;

namespace UTMarket.Application;

/// <summary>
/// Concretized use case implementation for searching sales with filters.
/// </summary>
public sealed class FetchSalesByFilterUseCaseImpl(ISaleRepository saleRepository) : IFetchSalesByFilterUseCase
{
    public IAsyncEnumerable<Sale> ExecuteAsync(SaleFilter filter, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(filter);
        return saleRepository.FindAsync(filter, ct);
    }
}
