using System;

namespace UTMarket.Core.Entities;

/// <summary>
/// Criteria object for filtering sales, designed for AOT compatibility.
/// Uses C# 14 Primary Constructor and validation via the 'field' keyword.
/// </summary>
public sealed record SaleFilter
{
    public DateTime? StartDate { get; init; }
    
    public DateTime? EndDate 
    { 
        get => field; 
        init => field = value >= StartDate ? value : throw new ArgumentException("EndDate cannot be earlier than StartDate"); 
    }

    public string? Folio { get; init; }
    public SaleStatus? Status { get; init; }
    public decimal? MinTotal { get; init; }
}
