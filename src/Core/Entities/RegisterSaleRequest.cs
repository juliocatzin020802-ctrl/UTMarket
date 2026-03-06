using System.Collections.Generic;

namespace UTMarket.Core.Entities;

/// <summary>
/// DTO describing an individual product item within a sale registration request.
/// </summary>
public record ProductSaleInfo(int ProductId, int Quantity);

/// <summary>
/// DTO representing a request to register a complete sale transaction.
/// </summary>
public record RegisterSaleRequest(int? CustomerId, List<ProductSaleInfo> Items);
