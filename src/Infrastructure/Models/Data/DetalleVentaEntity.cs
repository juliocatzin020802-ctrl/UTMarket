namespace UTMarket.Infrastructure.Models.Data;

/// <summary>
/// Data entity for Sale Detail, precisely mapping to the dbo.DetalleVenta table schema.
/// Designed for Dapper AOT compatibility according to prompt 03-dotnet-10-dapper-aot-entities.md.
/// </summary>
public partial class DetalleVentaEntity(int detalleId, int ventaId, int productoId)
{
    public int DetalleID { get; } = detalleId;
    public int VentaID { get; } = ventaId;
    public int ProductoID { get; } = productoId;

    private decimal _precioUnitario;
    public decimal PrecioUnitario
    {
        get => _precioUnitario;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(PrecioUnitario), "PrecioUnitario cannot be negative.");
            _precioUnitario = value;
        }
    }

    private int _cantidad;
    public int Cantidad
    {
        get => _cantidad;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(Cantidad), "Cantidad must be greater than 0.");
            _cantidad = value;
        }
    }
    
    public decimal TotalDetalle { get; set; }
}
