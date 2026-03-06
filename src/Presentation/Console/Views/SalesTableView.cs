using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UTMarket.Core.Entities;

namespace UTMarket.Presentation.Console.Views;

/// <summary>
/// Renders a table view of sales data to the console.
/// Implemented based on prompt 15_Ejercicio_03.txt.
/// </summary>
public static class SalesTableView
{
    public static async Task RenderAsync(IAsyncEnumerable<Sale> sales)
    {
        System.Console.WriteLine("==================================================================");
        System.Console.WriteLine("| Folio            | Fecha                | Monto Total         |");
        System.Console.WriteLine("==================================================================");

        decimal grandTotal = 0;
        int recordCount = 0;
        bool anySales = false;

        await foreach (var sale in sales)
        {
            System.Console.WriteLine($"| {sale.Folio,-16} | {sale.SaleDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),-20} | {sale.TotalSale.ToString("C2", CultureInfo.CurrentCulture),-19} |");
            grandTotal += sale.TotalSale;
            recordCount++;
            anySales = true;
        }

        if (!anySales)
        {
            System.Console.WriteLine("|                               No se encontraron ventas.                                 |");
        }
        else
        {
            System.Console.WriteLine("==================================================================");
            System.Console.WriteLine($"| Total de Ventas: {recordCount,-3} | GRAN TOTAL: {grandTotal.ToString("C2", CultureInfo.CurrentCulture),-20} |");
        }
        System.Console.WriteLine("==================================================================");
    }
}
