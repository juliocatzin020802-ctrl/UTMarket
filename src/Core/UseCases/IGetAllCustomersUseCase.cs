using System.Collections.Generic;
using System.Threading;
using UTMarket.Core.Entities;

namespace UTMarket.Core.UseCases;

/// <summary>
/// Use case for retrieving all customers.
/// </summary>
public interface IGetAllCustomersUseCase
{
    /// <summary>
    /// Executes the process of fetching all customers.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>An async collection of customers.</returns>
    IAsyncEnumerable<Customer> ExecuteAsync(CancellationToken ct = default);
}
