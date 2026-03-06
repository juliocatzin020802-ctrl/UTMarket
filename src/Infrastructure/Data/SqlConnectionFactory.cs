using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using UTMarket.Core.Abstractions;
using UTMarket.Infrastructure.Settings;

namespace UTMarket.Infrastructure.Data;

/// <summary>
/// Concrete implementation of IDbConnectionFactory for SQL Server.
/// </summary>
public sealed class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IOptions<DatabaseSettings> dbSettings)
    {
        _connectionString = dbSettings.Value.DefaultConnection;
        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            throw new InvalidOperationException("La cadena de conexiÃ³n 'DefaultConnection' no ha sido configurada correctamente en appsettings.json o Secrets.");
        }
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}
