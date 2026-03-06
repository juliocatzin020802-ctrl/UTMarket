using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UTMarket.Core.Entities;
using UTMarket.Core.UseCases;
using UTMarket.Presentation.Console.Utils; // For InputHelpers

namespace UTMarket.Presentation.Console.Orchestrators;

/// <summary>
/// Orchestrates the process of fetching and displaying sales history based on user input.
/// Implemented based on prompt 15_Ejercicio_03.txt.
/// </summary>
public class SalesHistoryOrchestrator(
    IFetchSalesByFilterUseCase fetchSalesByFilterUseCase)
{
    public async IAsyncEnumerable<Sale> ExecuteAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        System.Console.WriteLine("\n--- Consultar Ventas por Fecha ---");
        
        var (startDate, endDate) = InputHelpers.ReadDateRange();

        var filter = new SaleFilter { StartDate = startDate, EndDate = endDate };
        System.Console.WriteLine("Buscando ventas...");
        
        await foreach (var sale in fetchSalesByFilterUseCase.ExecuteAsync(filter, ct))
        {
            yield return sale;
        }
    }
}
