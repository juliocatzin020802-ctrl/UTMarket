namespace UTMarket.Infrastructure.Models.Data;

/// <summary>
/// Data entity for Product, precisely mapping to the dbo.Producto table schema.
/// Designed for Dapper AOT compatibility according to prompt 03-dotnet-10-dapper-aot-entities.md.
/// </summary>
public partial class ProductoEntity(int productoId, string sku)
{
    public int ProductoID { get; } = productoId;
    public string SKU { get; } = sku;

    public string Nombre { get; set; } = string.Empty;
    public string? Marca { get; set; }

    private decimal _precio;
    public decimal Precio
    {
        get => _precio;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(Precio), "Precio cannot be negative.");
            _precio = value;
        }
    }

    private int _stock;
    public int Stock
    {
        get => _stock;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(Stock), "Stock cannot be negative.");
            _stock = value;
        }
    }
}
