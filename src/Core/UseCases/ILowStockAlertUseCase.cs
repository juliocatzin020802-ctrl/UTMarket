using System.Collections.Generic;
using System.Threading;
using UTMarket.Core.Entities;

namespace UTMarket.Core.UseCases;

/// <summary>
/// Defines the contract for the low stock alert use case.
/// Updated based on prompt 15_Ejercicio02.txt.
/// </summary>
public interface ILowStockAlertUseCase
{
    /// <summary>
    /// Identifies products whose stock is at or below a given threshold.
    /// </summary>
    /// <param name="threshold">The critical stock level. Products with stock at or below this value will be returned.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the work.</param>
    /// <returns>An asynchronous stream of products that are low in stock.</returns>
    IAsyncEnumerable<Product> ExecuteAsync(int threshold, CancellationToken ct = default);
}
