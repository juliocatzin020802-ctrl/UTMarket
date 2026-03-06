namespace UTMarket.Core.Entities;

/// <summary>
/// Represents a complete sales transaction in the UTMarket domain.
/// Designed according to prompt 02-dotnet-10-entity-architect.md and adjusted for mapping.
/// </summary>
public class Sale(string folio)
{
    // Primary constructor for new sales, where ID is not yet known
    public Sale(string folio, DateTime saleDate) : this(folio)
    {
        SaleDate = saleDate;
    }

    // Secondary constructor for existing sales, where ID and Date are known
    public Sale(int saleId, string folio, DateTime saleDate) : this(folio, saleDate)
    {
        SaleId = saleId;
    }

    public int SaleId { get; set; } // Made settable for mapper
    public string Folio { get; } = folio ?? throw new ArgumentNullException(nameof(folio));

    private DateTime _saleDate = DateTime.Now;
    /// <summary>
    /// Exact date and time of the sale. Can be set for mapping existing sales.
    /// </summary>
    public DateTime SaleDate { get => _saleDate; set => _saleDate = value; } // Made settable for mapper

    /// <summary>
    /// Current status of the sale.
    /// </summary>
    public SaleStatus Status { get; set; } = SaleStatus.Pending;

    private readonly List<SaleDetail> _details = new();
    /// <summary>
    /// Read-only collection of sale details (captured products).
    /// </summary>
    public IReadOnlyList<SaleDetail> Details => _details.AsReadOnly();

    /// <summary>
    /// Total sum of units sold in this transaction.
    /// </summary>
    public int TotalItems => _details.Sum(d => d.Quantity);

    /// <summary>
    /// Total monetary sum of the sale (dynamic aggregation of details).
    /// </summary>
    public decimal TotalSale => _details.Sum(d => d.TotalDetail);

    /// <summary>
    /// Adds a product detail to the sale.
    /// </summary>
    /// <param name="detail">The sale detail to add.</param>
    public void AddDetail(SaleDetail detail)
    {
        if (detail is null) throw new ArgumentNullException(nameof(detail));
        _details.Add(detail);
    }
}
