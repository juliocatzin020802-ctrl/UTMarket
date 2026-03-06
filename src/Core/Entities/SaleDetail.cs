namespace UTMarket.Core.Entities;

/// <summary>
/// Individual detail of a Sale, capturing the historical price.
/// Designed according to prompt 02-dotnet-10-entity-architect.md and adjusted for mapping.
/// </summary>
public class SaleDetail(int saleDetailId, Product product, int quantity)
{
    public int SaleDetailId { get; } = saleDetailId; // Made get-only and initialized from constructor
    public Product Product { get; } = product ?? throw new ArgumentNullException(nameof(product));
    
    /// <summary>
    /// Unit price captured at the time of sale to preserve history.
    /// </summary>
    public decimal UnitPrice { get; } = product.Price;

    private int _quantity = quantity; // Initialize _quantity here
    /// <summary>
    /// Number of units sold. Must be greater than zero.
    /// </summary>
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Quantity), "Quantity must be greater than zero.");
            }
            _quantity = value;
        }
    }

    /// <summary>
    /// Calculated total for the detail line (UnitPrice * Quantity).
    /// </summary>
    public decimal TotalDetail => UnitPrice * Quantity;
}
