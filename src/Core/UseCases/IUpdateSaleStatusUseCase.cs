using UTMarket.Core.Entities;

namespace UTMarket.Core.UseCases;

/// <summary>
/// Use case for updating strictly the status of an existing sale.
/// </summary>
public interface IUpdateSaleStatusUseCase
{
    /// <summary>
    /// Executes the partial update of a sale's status.
    /// </summary>
    /// <param name="id">Sale identifier.</param>
    /// <param name="newStatus">The new status to apply.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    Task ExecuteAsync(int id, SaleStatus newStatus, CancellationToken ct = default);
}
