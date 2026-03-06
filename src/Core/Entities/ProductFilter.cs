namespace UTMarket.Core.Entities;

/// <summary>
/// Criterios de filtrado para bÃºsqueda de productos (Compatible con Native AOT).
/// </summary>
public sealed record ProductFilter
{
    public string? NameContains { get; init; }
    public string? Brand { get; init; }
    public string? SKU { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
}
