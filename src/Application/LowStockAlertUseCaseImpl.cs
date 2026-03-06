using System;
using System.Collections.Generic;
using System.Threading;
using UTMarket.Core.Entities;
using UTMarket.Core.Repositories;
using UTMarket.Core.UseCases;

namespace UTMarket.Application;

/// <summary>
/// Implementation of the use case for low stock alerts.
/// Updated based on prompt 15_Ejercicio02.txt.
/// </summary>
public sealed class LowStockAlertUseCaseImpl(IProductRepository productRepository) : ILowStockAlertUseCase
{
    public async IAsyncEnumerable<Product> ExecuteAsync(int threshold, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        if (threshold < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(threshold), "Threshold cannot be negative.");
        }

        // The repository returns a stream, and we filter it as it comes in.
        // This is memory-efficient as we don't load all products into a list.
        await foreach (var product in productRepository.GetAllAsync(ct))
        {
            if (product.Stock <= threshold)
            {
                yield return product;
            }
        }
    }
}
