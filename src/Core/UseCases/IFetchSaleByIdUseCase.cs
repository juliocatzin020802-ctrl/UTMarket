using UTMarket.Core.Entities;

namespace UTMarket.Core.UseCases;

/// <summary>
/// Use case for fetching a specific sale by its unique identifier.
/// </summary>
public interface IFetchSaleByIdUseCase
{
    /// <summary>
    /// Executes the search for a sale by ID.
    /// </summary>
    /// <param name="id">Sale identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The sale if found; otherwise, null.</returns>
    Task<Sale?> ExecuteAsync(int id, CancellationToken ct = default);
}
