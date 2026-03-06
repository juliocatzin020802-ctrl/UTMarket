using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using UTMarket.Core.Entities;

namespace UTMarket.Core.Repositories;

/// <summary>
/// Domain repository contract for managing the 'Sale' aggregate root.
/// Optimized for Native AOT and high-performance data streaming in .NET 10.
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Retrieves all sales as an asynchronous stream to minimize memory footprint.
    /// </summary>
    /// <param name="cancellationToken">Cooperative cancellation token.</param>
    /// <returns>An async stream of domain 'Sale' objects.</returns>
    IAsyncEnumerable<Sale> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a complete sale by its unique persistence identifier.
    /// </summary>
    /// <param name="id">The sale's persistence ID.</param>
    /// <param name="cancellationToken">Cooperative cancellation token.</param>
    /// <param name="transaction">Optional database transaction.</param>
    /// <returns>A 'Sale' domain object with its details populated, or null if not found.</returns>
    Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken = default, IDbTransaction? transaction = null);

    /// <summary>
    /// Searches for sales based on specific criteria without using dynamic expressions.
    /// </summary>
    /// <param name="filter">The search criteria object.</param>
    /// <param name="cancellationToken">Cooperative cancellation token.</param>
    /// <returns>An async stream of sales matching the criteria.</returns>
    IAsyncEnumerable<Sale> FindAsync(SaleFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists a new sale aggregate into the store.
    /// </summary>
    /// <param name="sale">The domain sale object to persist.</param>
    /// <param name="cancellationToken">Cooperative cancellation token.</param>
    /// <param name="transaction">Optional database transaction.</param>
    /// <returns>The persisted sale with its generated identity.</returns>
    Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken = default, IDbTransaction? transaction = null);

    /// <summary>
    /// Updates an existing sale aggregate (including its details).
    /// </summary>
    /// <param name="sale">The domain sale object with updated data.</param>
    /// <param name="cancellationToken">Cooperative cancellation token.</param>
    /// <param name="transaction">Optional database transaction.</param>
    /// <returns>A task representing the update operation.</returns>
    Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default, IDbTransaction? transaction = null);
}
