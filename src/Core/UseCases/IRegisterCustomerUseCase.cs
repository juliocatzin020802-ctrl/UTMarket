using System.Threading;
using System.Threading.Tasks;
using UTMarket.Core.Entities;

namespace UTMarket.Core.UseCases;

/// <summary>
/// Use case for registering a new customer.
/// </summary>
public interface IRegisterCustomerUseCase
{
    /// <summary>
    /// Executes the customer registration process.
    /// </summary>
    /// <param name="customer">The customer to register.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The registered customer with its generated ID.</returns>
    Task<Customer> ExecuteAsync(Customer customer, CancellationToken ct = default);
}
