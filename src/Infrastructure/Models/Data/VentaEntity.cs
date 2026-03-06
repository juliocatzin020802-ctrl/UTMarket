namespace UTMarket.Infrastructure.Models.Data;

/// <summary>
/// Data entity for Sale, precisely mapping to the dbo.Venta table schema.
/// Designed for Dapper AOT compatibility according to prompt 03-dotnet-10-dapper-aot-entities.md.
/// </summary>
public partial class VentaEntity(int ventaId, string folio)
{
    public int VentaID { get; } = ventaId;
    public string Folio { get; } = folio;

    public DateTime FechaVenta { get; set; }
    public int TotalArticulos { get; set; }
    public decimal TotalVenta { get; set; }
    
    private byte _estatus;
    /// <summary>
    /// Sale status: 1=Pending, 2=Completed, 3=Cancelled
    /// </summary>
    public byte Estatus 
    {
        get => _estatus;
        set
        {
            if (value < 1 || value > 3)
                throw new ArgumentOutOfRangeException(nameof(Estatus), "Estatus must be between 1 and 3.");
            _estatus = value;
        }
    }
}
