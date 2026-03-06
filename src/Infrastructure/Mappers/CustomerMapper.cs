using UTMarket.Core.Entities;
using UTMarket.Infrastructure.Models.Data;

namespace UTMarket.Infrastructure.Mappers;

/// <summary>
/// Maps between Customer domain objects and CustomerEntity data objects.
/// </summary>
public static class CustomerMapper
{
    /// <summary>
    /// Transforms a CustomerEntity to a Customer domain object.
    /// </summary>
    public static Customer ToDomain(this CustomerEntity entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        var customer = new Customer(entity.CustomerId)
        {
            FullName = entity.FullName,
            IsActive = entity.IsActive
        };
        customer.Email = entity.Email; // Use setter for validation
        return customer;
    }

    /// <summary>
    /// Transforms a Customer domain object to a CustomerEntity.
    /// </summary>
    public static CustomerEntity ToEntity(this Customer domain)
    {
        if (domain is null) throw new ArgumentNullException(nameof(domain));

        return new CustomerEntity(domain.CustomerId, domain.Email)
        {
            FullName = domain.FullName,
            IsActive = domain.IsActive
        };
    }
}
