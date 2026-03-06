using UTMarket.Core.Entities;
using UTMarket.Infrastructure.Models.Data;

namespace UTMarket.Infrastructure.Mappers;

/// <summary>
/// Puente de datos ultra-eficiente entre la capa de persistencia (ProductoEntity) y el Dominio (Product).
/// Optimizado para Native AOT y Trimming en .NET 10.
/// </summary>
public static class ProductMapper
{
    /// <summary>
    /// Transforma una entidad de base de datos 'ProductoEntity' al objeto de dominio 'Product'.
    /// </summary>
    /// <param name="entity">Entidad de persistencia de origen.</param>
    /// <returns>Objeto de dominio 'Product'.</returns>
    public static Product ToDomain(this ProductoEntity entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        return new Product(entity.ProductoID, entity.SKU)
        {
            Name = entity.Nombre,
            Brand = entity.Marca,
            Price = entity.Precio,
            Stock = entity.Stock
        };
    }

    /// <summary>
    /// Transforma un objeto de dominio 'Product' a la entidad de persistencia 'ProductoEntity'.
    /// </summary>
    /// <param name="domain">Objeto de dominio de origen.</param>
    /// <returns>Entidad de base de datos 'ProductoEntity'.</returns>
    public static ProductoEntity ToEntity(this Product domain)
    {
        if (domain is null) throw new ArgumentNullException(nameof(domain));

        return new ProductoEntity(domain.ProductID, domain.SKU)
        {
            Nombre = domain.Name,
            Marca = domain.Brand,
            Precio = domain.Price,
            Stock = domain.Stock
        };
    }
}
// C# 14 Extension Blocks (Conceptual for .NET 10 preview context/Role-based design)
// In some early builds of C# 14, this could be represented as:
/*
public extension ProductExtension for Product 
{
    public ProductoEntity ToEntity() => ...
}
*/
