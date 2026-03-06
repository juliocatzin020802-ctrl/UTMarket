namespace UTMarket.Core.Entities;

/// <summary>
/// Represents a Customer in the domain.
/// Updated based on prompt 15_Ejercicio01.txt.
/// </summary>
public class Customer(int customerId)
{
    public int CustomerId { get; } = customerId;
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    /// <summary>
    /// The customer's email address. The format is validated on set.
    /// </summary>
    public string Email
    {
        get => field;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains('@') || !value.Contains('.'))
            {
                throw new ArgumentException("A valid email format (containing '@' and '.') is required.", nameof(Email));
            }
            field = value;
        }
    } = string.Empty;
}
