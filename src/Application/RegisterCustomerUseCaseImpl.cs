using System.Threading;
using System.Threading.Tasks;
using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Core.UseCases;

namespace UTMarket.Application;

/// <summary>
/// Implementation of the register customer use case.
/// </summary>
public sealed class RegisterCustomerUseCaseImpl(ICustomerRepository customerRepository) : IRegisterCustomerUseCase
{
    public async Task<Customer> ExecuteAsync(Customer customer, CancellationToken ct = default)
    {
        // Business Rule: Validate if customer already exists by email
        var existing = await customerRepository.GetByEmailAsync(customer.Email);
        if (existing is not null)
        {
            throw new InvalidOperationException($"A customer with email '{customer.Email}' already exists.");
        }

        var id = await customerRepository.AddAsync(customer);
        
        // Return a new customer instance with the generated ID (or the same one if ID was set in repo)
        return new Customer(id)
        {
            FullName = customer.FullName,
            Email = customer.Email,
            IsActive = customer.IsActive
        };
    }
}
