using UTMarket.Core.Entities;

namespace UTMarket.Core.UseCases;

/// <summary>
/// Use case for fetching sales based on domain-driven filtering criteria.
/// </summary>
public interface IFetchSalesByFilterUseCase
{
    /// <summary>
    /// Executes the filtered search of sales.
    /// </summary>
    /// <param name="filter">Sale filtering criteria.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Async stream of sales matching the criteria.</returns>
    IAsyncEnumerable<Sale> ExecuteAsync(SaleFilter filter, CancellationToken ct = default);
}
