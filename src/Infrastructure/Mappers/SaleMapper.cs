using System;
using System.Linq;
using System.Collections.Generic;
using UTMarket.Core.Entities;
using UTMarket.Infrastructure.Models.Data;

namespace UTMarket.Infrastructure.Mappers;

/// <summary>
/// Orchestrates ultra-efficient mapping for sale transactions and their details.
/// Optimized for Native AOT and reflection-free in .NET 10.
/// Updated to match the refactored Sale entity.
/// </summary>
public static class SaleMapper
{
    /// <summary>
    /// Converts an Infrastructure Sale to a Domain Sale.
    /// Requires a dictionary or list of products to correctly reconstruct the SaleDetail object.
    /// </summary>
    public static Sale ToDomain(this VentaEntity entity, IEnumerable<DetalleVentaEntity> details, IDictionary<int, Product> productCache)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));
        if (details is null) throw new ArgumentNullException(nameof(details));

        // Use the new constructor for Sale
        var sale = new Sale(entity.VentaID, entity.Folio, entity.FechaVenta)
        {
            Status = (SaleStatus)entity.Estatus
        };

        foreach (var detailEntity in details)
        {
            if (productCache.TryGetValue(detailEntity.ProductoID, out var product))
            {
                // Reconstruct the detail ensuring the fidelity of the historical price
                var detail = new SaleDetail(detailEntity.DetalleID, product, detailEntity.Cantidad); // Assuming SaleDetail takes SaleDetailId in constructor now
                // Use the AddDetail method
                sale.AddDetail(detail);
            }
        }

        return sale;
    }

    /// <summary>
    /// Converts a Domain Sale to its Persistence representation.
    /// </summary>
    public static (VentaEntity Venta, IEnumerable<DetalleVentaEntity> Detalles) ToEntity(this Sale domain)
    {
        if (domain is null) throw new ArgumentNullException(nameof(domain));

        var ventaEntity = new VentaEntity(domain.SaleId, domain.Folio)
        {
            FechaVenta = domain.SaleDate,
            Estatus = (byte)domain.Status // Explicit cast for byte
        };

        var detallesEntities = domain.Details.Select(d => new DetalleVentaEntity(d.SaleDetailId, domain.SaleId, d.Product.ProductID)
        {
            Cantidad = d.Quantity,
            PrecioUnitario = d.UnitPrice,
            TotalDetalle = d.TotalDetail
        });

        return (ventaEntity, detallesEntities);
    }
}
