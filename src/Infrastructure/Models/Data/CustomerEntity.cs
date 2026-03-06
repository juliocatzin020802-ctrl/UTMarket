namespace UTMarket.Infrastructure.Models.Data;

/// <summary>
/// Data entity for Customer, mapping to dbo.Customers table.
/// </summary>
public partial class CustomerEntity(int customerId, string email)
{
    public int CustomerId { get; init; } = customerId;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; init; } = email;
    public bool IsActive { get; set; } = true;
}
