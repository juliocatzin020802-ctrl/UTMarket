using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using UTMarket.Core.Abstractions;
using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Infrastructure.Mappers;
using UTMarket.Infrastructure.Models.Data;

namespace UTMarket.Infrastructure.Repositories;

/// <summary>
/// Concrete repository for Customers using ADO.NET and manual mapping for Native AOT compatibility.
/// Updated based on prompt 15_Ejercicio01.txt.
/// </summary>
public sealed class SqlCustomerRepository(IDbConnectionFactory dbFactory) : ICustomerRepository
{
    public async Task<Customer?> GetByEmailAsync(string email)
    {
        using var connection = await dbFactory.CreateConnectionAsync();
        const string sql = "SELECT CustomerId, FullName, Email, IsActive FROM dbo.Customers WHERE Email = @Email;";

        await using var command = new SqlCommand(sql, (SqlConnection)connection);
        command.Parameters.AddWithValue("@Email", email);

        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);
        if (await reader.ReadAsync())
        {
            return MapToCustomerEntity(reader).ToDomain();
        }

        return null;
    }

    public async Task<int> AddAsync(Customer customer)
    {
        using var connection = await dbFactory.CreateConnectionAsync();
        const string sql = @"
            INSERT INTO dbo.Customers (FullName, Email, IsActive) 
            OUTPUT INSERTED.CustomerId
            VALUES (@FullName, @Email, @IsActive);";

        var entity = customer.ToEntity();
        await using var command = new SqlCommand(sql, (SqlConnection)connection);
        command.Parameters.AddWithValue("@FullName", entity.FullName);
        command.Parameters.AddWithValue("@Email", entity.Email);
        command.Parameters.AddWithValue("@IsActive", entity.IsActive);

        var newId = Convert.ToInt32(await command.ExecuteScalarAsync());
        return newId;
    }

    public async IAsyncEnumerable<Customer> GetAllAsync()
    {
        using var connection = await dbFactory.CreateConnectionAsync();
        const string sql = "SELECT CustomerId, FullName, Email, IsActive FROM dbo.Customers;";

        await using var command = new SqlCommand(sql, (SqlConnection)connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            yield return MapToCustomerEntity(reader).ToDomain();
        }
    }

    private static CustomerEntity MapToCustomerEntity(SqlDataReader reader)
    {
        return new CustomerEntity(reader.GetInt32(0), reader.GetString(2))
        {
            FullName = reader.GetString(1),
            IsActive = reader.GetBoolean(3)
        };
    }
}

