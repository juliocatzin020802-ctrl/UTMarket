using UTMarket.Core.Entities;

namespace UTMarket.Application;

/// <summary>
/// Helper para capturar datos de producto desde la consola con validaciÃ³n.
/// </summary>
public static class ProductConsoleHelper
{
    public static Product? CaptureProductFromConsole()
    {
        Console.WriteLine("\n--- Registrar Nuevo Producto ---");

        string? name = ReadString("Nombre: ");
        if (name is null) return null;

        string? sku = ReadString("SKU: ");
        if (sku is null) return null;

        string? brand = ReadString("Marca: ");
        if (brand is null) return null;

        decimal price = ReadDecimal("Precio: ");
        int stock = ReadInt("Stock Inicial: ");

        return new Product(0, sku)
        {
            Name = name,
            Brand = brand,
            Price = price,
            Stock = stock
        };
    }

    public static string? ReadString(string prompt)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? null : input;
    }

    public static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (decimal.TryParse(Console.ReadLine(), out decimal result) && result >= 0)
            {
                return result;
            }
            Console.WriteLine("Error: Ingrese un nÃºmero decimal vÃ¡lido y no negativo.");
        }
    }

    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int result) && result >= 0)
            {
                return result;
            }
            Console.WriteLine("Error: Ingrese un nÃºmero entero vÃ¡lido y no negativo.");
        }
    }

    public static DateTime ReadDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (DateTime.TryParse(Console.ReadLine(), out DateTime result))
            {
                return result;
            }
            Console.WriteLine("Error: Ingrese una fecha válida (ej. yyyy-mm-dd).");
        }
    }

    public static RegisterSaleRequest? CaptureSaleFromConsole()
    {
        Console.WriteLine("\n--- Registrar Nueva Venta ---");
        var items = new List<ProductSaleInfo>();
        bool adding = true;

        while (adding)
        {
            int productId = ReadInt("ID del Producto: ");
            int quantity = ReadInt("Cantidad: ");
            if (quantity <= 0)
            {
                Console.WriteLine("Error: La cantidad debe ser mayor a cero.");
                continue;
            }

            items.Add(new ProductSaleInfo(productId, quantity));

            Console.Write("¿Desea agregar otro producto? (s/n): ");
            var response = Console.ReadLine()?.ToLower();
            adding = response == "s";
        }

        if (items.Count == 0)
        {
            Console.WriteLine("Error: No se agregaron productos a la venta.");
            return null;
        }

        return new RegisterSaleRequest(null, items);
    }

    public static Customer? CaptureCustomerFromConsole()
    {
        Console.WriteLine("\n--- Registrar Nuevo Cliente ---");

        string? fullName = ReadString("Nombre Completo: ");
        if (fullName is null) return null;

        string? email = ReadString("Correo Electrónico: ");
        if (email is null) return null;

        return new Customer(0)
        {
            FullName = fullName,
            Email = email,
            IsActive = true
        };
    }
}
