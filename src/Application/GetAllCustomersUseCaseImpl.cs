using System.Collections.Generic;
using System.Threading;
using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Core.UseCases;

namespace UTMarket.Application;

/// <summary>
/// Implementation of the use case to fetch all customers.
/// </summary>
public sealed class GetAllCustomersUseCaseImpl(ICustomerRepository customerRepository) : IGetAllCustomersUseCase
{
    public IAsyncEnumerable<Customer> ExecuteAsync(CancellationToken ct = default)
    {
        return customerRepository.GetAllAsync();
    }
}
