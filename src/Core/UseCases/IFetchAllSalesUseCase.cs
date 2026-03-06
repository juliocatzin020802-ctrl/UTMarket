using UTMarket.Core.Entities;

namespace UTMarket.Core.UseCases;

/// <summary>
/// Use case for fetching all sale records as an asynchronous stream.
/// </summary>
public interface IFetchAllSalesUseCase
{
    /// <summary>
    /// Executes the retrieval of all sales.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Async stream of Sale domain objects.</returns>
    IAsyncEnumerable<Sale> ExecuteAsync(CancellationToken ct = default);
}
