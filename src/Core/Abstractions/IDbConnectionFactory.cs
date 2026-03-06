using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace UTMarket.Core.Abstractions;

/// <summary>
/// Defines a factory for creating database connections.
/// This abstraction decouples domain logic from specific data providers.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Creates and opens a new database connection asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An open <see cref="IDbConnection"/>.</returns>
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}
