using System.Threading;
using System.Threading.Tasks;
using UTMarket.Core.Entities;

namespace UTMarket.Core.UseCases;

/// <summary>
/// Domain contract for the sale registration use case.
/// Orchestrates sale creation and stock updates in an atomic transaction.
/// </summary>
public interface IRegisterSaleUseCase
{
    /// <summary>
    /// Executes the sale registration process.
    /// </summary>
    /// <param name="request">The sale request data.</param>
    /// <param name="cancellationToken">Cooperative cancellation token.</param>
    /// <returns>The registered <see cref="Sale"/> aggregate.</returns>
    Task<Sale> ExecuteAsync(RegisterSaleRequest request, CancellationToken cancellationToken = default);
}
