namespace UTMarket.Core.Entities;

/// <summary>
/// Pure domain entity representing a Product in the UTMarket catalog.
/// Corrected property names to PascalCase to match project conventions and fix build errors.
/// </summary>
public class Product(int productId, string sku)
{
    public int ProductID { get; } = productId; // Corrected from ProductId
    public string SKU { get; } = sku; // Corrected from Sku
    
    public string Name { get; set; } = string.Empty;
    public string? Brand { get; set; }

    private decimal _price;
    /// <summary>
    /// Sale price. Cannot be negative.
    /// </summary>
    public decimal Price
    {
        get => _price;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Price), "Product price cannot be negative.");
            }
            _price = value;
        }
    }

    private int _stock;
    /// <summary>
    /// Available quantity in warehouse. Cannot be negative.
    /// </summary>
    public int Stock
    {
        get => _stock;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Stock), "Product stock cannot be negative.");
            }
            _stock = value;
        }
    }
}
