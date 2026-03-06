using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UTMarket.Core.Abstractions;
using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Core.UseCases;

namespace UTMarket.Application;

/// <summary>
/// Implementation of the RegisterSale use case.
/// Orchestrates the stock validation, sale creation, and stock reduction within a single transaction.
/// </summary>
public sealed class RegisterSaleUseCaseImpl(
    IProductRepository productRepository,
    ISaleRepository saleRepository,
    IDbConnectionFactory dbConnectionFactory) : IRegisterSaleUseCase
{
    public async Task<Sale> ExecuteAsync(RegisterSaleRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Items == null || !request.Items.Any())
        {
            throw new ArgumentException("A sale must contain at least one item.", nameof(request));
        }

        using var connection = await dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        try
        {
            // 1. Prepare Sale object and validate stock
            var sale = new Sale($"V-{DateTime.Now:yyyyMMdd-HHmmss}");
            
            foreach (var item in request.Items)
            {
                var product = await productRepository.GetByIdAsync(item.ProductId, cancellationToken, transaction);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with ID {item.ProductId} not found.");
                }

                if (product.Stock < item.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for product '{product.Name}'. Available: {product.Stock}, Requested: {item.Quantity}.");
                }

                // Prepare detail
                var detail = new SaleDetail(0, product, item.Quantity);
                sale.AddDetail(detail);

                // Update (Reduce) Product Stock
                var newStock = product.Stock - item.Quantity;
                await productRepository.UpdateStockAsync(item.ProductId, newStock, cancellationToken, transaction);
            }

            // 2. Persist Sale and Details
            var persistedSale = await saleRepository.AddAsync(sale, cancellationToken, transaction);

            // 3. Commit transaction
            transaction.Commit();

            return persistedSale;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
}
