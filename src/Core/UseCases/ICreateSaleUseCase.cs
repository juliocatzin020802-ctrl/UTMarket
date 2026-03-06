using UTMarket.Core.Entities;

namespace UTMarket.Core.UseCases;

/// <summary>
/// Use case for orchestrating the creation and persistence of a new sale.
/// </summary>
public interface ICreateSaleUseCase
{
    /// <summary>
    /// Executes the sale creation process.
    /// </summary>
    /// <param name="sale">The sale domain aggregate to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created sale with its generated identity.</returns>
    Task<Sale> ExecuteAsync(Sale sale, CancellationToken ct = default);
}
