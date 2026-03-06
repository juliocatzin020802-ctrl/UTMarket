using System.Threading.Tasks;
using UTMarket.Core.Entities;

namespace UTMarket.Core.Repositories;

/// <summary>
/// Persistence contract for customers.
/// Updated based on prompt 15_Ejercicio01.txt.
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Finds a customer by their email address.
    /// </summary>
    /// <param name="email">The email to search for.</param>
    /// <returns>The customer if found; otherwise, null.</returns>
    Task<Customer?> GetByEmailAsync(string email);

    /// <summary>
    /// Registers a new customer in the system.
    /// </summary>
    /// <param name="customer">The customer object to persist.</param>
    /// <returns>The ID of the newly created customer.</returns>
    Task<int> AddAsync(Customer customer);

    /// <summary>
    /// Retrieves all customers in the system.
    /// </summary>
    /// <returns>A collection of all customers.</returns>
    IAsyncEnumerable<Customer> GetAllAsync();
}
