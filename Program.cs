using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UTMarket.Infrastructure;
using UTMarket.Application;
using UTMarket.Core.UseCases;
using UTMarket.Core.Entities;
using System.Threading.Tasks;
using System.Threading;
using UTMarket.Presentation.Console;
using UTMarket.Presentation.Console.Orchestrators;
using UTMarket.Presentation.Console.Views;
using UTMarket.Presentation.Console.Utils;

// Configurar el Application Builder con soporte para secretos en Desarrollo
var builder = Host.CreateApplicationBuilder(args);

// Cargar secretos de usuario solo en entorno de Desarrollo
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Inyectar servicios de las capas de AplicaciÃ³n e Infraestructura
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPresentation();

// Registrar el orquestador principal
builder.Services.AddScoped<IMarketOrchestrator, MarketOrchestrator>();

using IHost host = builder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("UTMarket CLI - Entorno: {Env}", builder.Environment.EnvironmentName);

// Ejecutar el ciclo de vida principal dentro de un scope manual para servicios Scoped
using (var scope = host.Services.CreateScope())
{
    if (args.Length > 0 && args[0] == "--seed")
    {
        await SeedDatabaseAsync(scope.ServiceProvider);
        return;
    }

    var orchestrator = scope.ServiceProvider.GetRequiredService<IMarketOrchestrator>();
    await orchestrator.RunAsync(args, CancellationToken.None);
}

async Task SeedDatabaseAsync(IServiceProvider sp)
{
    var config = sp.GetRequiredService<IConfiguration>();
    var logger = sp.GetRequiredService<ILogger<Program>>();
    string connectionString = config.GetConnectionString("DefaultConnection") ?? "";

    try
    {
        Console.WriteLine("Iniciando proceso de inicialización de base de datos...");
        
        var connectionBuilder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString)
        {
            Encrypt = false,
            TrustServerCertificate = true
        };

        using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionBuilder.ConnectionString);
        await connection.OpenAsync();

        string[] scripts = { 
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "src", "database-scripts", "01_create_structure_utm_market.sql"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "src", "database-scripts", "02_seed_data_utm_market.sql")
        };

        foreach (var path in scripts)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Error: No se encontró el archivo {path}");
                continue;
            }

            Console.WriteLine($"Ejecutando: {Path.GetFileName(path)}");
            string script = await File.ReadAllTextAsync(path);
            
            // Separar por bloques GO
            var batches = System.Text.RegularExpressions.Regex.Split(script, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            foreach (var batch in batches)
            {
                if (string.IsNullOrWhiteSpace(batch)) continue;
                using var command = new Microsoft.Data.SqlClient.SqlCommand(batch, connection);
                await command.ExecuteNonQueryAsync();
            }
        }

        Console.WriteLine("Base de datos inicializada exitosamente.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error durante la inicialización de la base de datos.");
        Console.WriteLine($"Error fatal: {ex.Message}");
    }
}

public interface IMarketOrchestrator
{
    Task RunAsync(string[] args, CancellationToken ct);
}

public sealed class MarketOrchestrator(
    IServiceProvider serviceProvider,
    ILogger<MarketOrchestrator> logger) : IMarketOrchestrator
{
    public async Task RunAsync(string[] args, CancellationToken ct)
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("""
                =================================================
                   UTMARKET - PRODUCT MANAGEMENT SYSTEM (CLI)
                =================================================
                1. Consultar todos los productos
                2. Consultar producto por ID
                3. Registrar nuevo producto
                4. Consultar ventas por fecha
                5. Registrar nueva venta
                6. Registrar nuevo cliente
                7. Consultar todos los clientes
                8. Salir
                =================================================
                """);
            
            Console.Write("Seleccione una opcion: ");
            var option = Console.ReadLine();

            // The scope is created here for each menu option to ensure proper service lifetime management.
            using var scope = serviceProvider.CreateScope();
            var sp = scope.ServiceProvider;

            switch (option)
            {
                case "1":
                    await ListAllProductsAsync(sp.GetRequiredService<IGetAllProductsUseCase>(), ct);
                    break;
                case "2":
                    await GetProductByIdAsync(sp.GetRequiredService<IGetProductByIdUseCase>(), ct);
                    break;
                case "3":
                    await RegisterNewProductAsync(sp.GetRequiredService<ICreateProductUseCase>(), ct);
                    break;
                case "4":
                    await FetchSalesByDateAsync(sp.GetRequiredService<IFetchSalesByFilterUseCase>(), ct);
                    break;
                case "5":
                    await RegisterNewSaleAsync(sp.GetRequiredService<IRegisterSaleUseCase>(), ct);
                    break;
                case "6":
                    await RegisterNewCustomerAsync(sp.GetRequiredService<IRegisterCustomerUseCase>(), ct);
                    break;
                case "7":
                    await ListAllCustomersAsync(sp.GetRequiredService<IGetAllCustomersUseCase>(), ct);
                    break;
                case "8":
                    exit = true;
                    Console.WriteLine("Saliendo de la aplicacion...");
                    break;
                default:
                    Console.WriteLine("Opcion no valida. Presione cualquier tecla para reintentar.");
                    Console.ReadKey();
                    break;
            }

            if (!exit)
            {
                Console.WriteLine("\nPresione cualquier tecla para volver al menu...");
                Console.ReadKey();
            }
        }
    }

    private async Task ListAllProductsAsync(IGetAllProductsUseCase getAllProducts, CancellationToken ct)
    {
        Console.WriteLine("\n--- Listado de Productos ---");
        try
        {
            await foreach (var p in getAllProducts.ExecuteAsync(ct))
            {
                Console.WriteLine($"[ID: {p.ProductID, -4}] {p.Name, -30} | SKU: {p.SKU, -15} | Stock: {p.Stock, 5} | Price: {p.Price:C2}");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error crítico al listar productos desde el repositorio.");
            Console.WriteLine($"\n[ERROR] No se pudieron recuperar los productos.");
            Console.WriteLine($"Mensaje: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Detalle: {ex.InnerException.Message}");
            }
            Console.WriteLine("\nVerifique la conexión a la base de datos y que la tabla 'dbo.Producto' exista.");
        }
    }

    private async Task GetProductByIdAsync(IGetProductByIdUseCase getProductById, CancellationToken ct)
    {
        int id = ProductConsoleHelper.ReadInt("\nIngrese el ID del producto: ");
        
        try
        {
            var product = await getProductById.ExecuteAsync(id, ct);
            if (product is not null)
            {
                Console.WriteLine("\n--- Detalles del Producto ---");
                Console.WriteLine($"ID: {product.ProductID}");
                Console.WriteLine($"Nombre: {product.Name}");
                Console.WriteLine($"SKU: {product.SKU}");
                Console.WriteLine($"Marca: {product.Brand}");
                Console.WriteLine($"Precio: {product.Price:C2}");
                Console.WriteLine($"Stock: {product.Stock}");
            }
            else
            {
                Console.WriteLine($"\nEl producto con ID {id} no existe.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al buscar producto por ID.");
            Console.WriteLine("Ocurrio un error durante la busqueda.");
        }
    }

    private async Task RegisterNewProductAsync(ICreateProductUseCase createProduct, CancellationToken ct)
    {
        var newProduct = ProductConsoleHelper.CaptureProductFromConsole();
        if (newProduct is null)
        {
            Console.WriteLine("\nCancelando registro. Todos los campos son obligatorios.");
            return;
        }

        try
        {
            var created = await createProduct.ExecuteAsync(newProduct, ct);
            Console.WriteLine($"\nProducto '{created.Name}' registrado exitosamente con ID: {created.ProductID}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al registrar nuevo producto.");
            Console.WriteLine("\nError al guardar el producto. Verifique los datos.");
        }
    }

    private async Task RegisterNewSaleAsync(IRegisterSaleUseCase registerSale, CancellationToken ct)
    {
        var request = ProductConsoleHelper.CaptureSaleFromConsole();
        if (request is null) return;

        try
        {
            var result = await registerSale.ExecuteAsync(request, ct);
            Console.WriteLine($"\nVenta '{result.Folio}' registrada exitosamente con ID: {result.SaleId}");
            Console.WriteLine($"Total: {result.TotalSale:C2} ({result.TotalItems} articulos)");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\n[ERROR DE NEGOCIO] {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al registrar venta.");
            Console.WriteLine("\nOcurrio un error al procesar la venta.");
        }
    }

    private async Task FetchSalesByDateAsync(IFetchSalesByFilterUseCase fetchSales, CancellationToken ct)
    {
        Console.WriteLine("\n--- Consultar Ventas por Fecha ---");
        DateTime startDate = ProductConsoleHelper.ReadDate("Fecha de Inicio (yyyy-mm-dd): ");
        DateTime endDate = ProductConsoleHelper.ReadDate("Fecha de Fin (yyyy-mm-dd): ");

        try
        {
            var filter = new SaleFilter { StartDate = startDate, EndDate = endDate };
            
            Console.WriteLine("\n| Folio            | Fecha                | Monto Total |");
            Console.WriteLine("|------------------|----------------------|-------------|");

            bool found = false;
            await foreach (var sale in fetchSales.ExecuteAsync(filter, ct))
            {
                Console.WriteLine($"| {sale.Folio,-16} | {sale.SaleDate,-20} | {sale.TotalSale,11:C2} |");
                found = true;
            }

            if (!found)
            {
                Console.WriteLine("No se encontraron ventas en el rango especificado.");
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error de validacion: {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al consultar ventas por fecha.");
            Console.WriteLine("Ocurrio un error al recuperar las ventas.");
        }
    }

    private async Task RegisterNewCustomerAsync(IRegisterCustomerUseCase registerCustomer, CancellationToken ct)
    {
        try
        {
            var newCustomer = ProductConsoleHelper.CaptureCustomerFromConsole();
            if (newCustomer is null)
            {
                Console.WriteLine("\nCancelando registro. Todos los campos son obligatorios.");
                return;
            }

            var created = await registerCustomer.ExecuteAsync(newCustomer, ct);
            Console.WriteLine($"\nCliente '{created.FullName}' registrado exitosamente con ID: {created.CustomerId}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"\n[ERROR DE VALIDACIÓN] {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\n[ERROR DE NEGOCIO] {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al registrar nuevo cliente.");
            Console.WriteLine("\nOcurrio un error al guardar el cliente.");
        }
    }

    private async Task ListAllCustomersAsync(IGetAllCustomersUseCase getAllCustomers, CancellationToken ct)
    {
        Console.WriteLine("\n--- Listado de Clientes ---");
        try
        {
            bool found = false;
            await foreach (var c in getAllCustomers.ExecuteAsync(ct))
            {
                string status = c.IsActive ? "Activo" : "Inactivo";
                Console.WriteLine($"[ID: {c.CustomerId, -4}] {c.FullName, -30} | Email: {c.Email, -35} | Status: {status}");
                found = true;
            }

            if (!found)
            {
                Console.WriteLine("No hay clientes registrados.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al listar clientes.");
            Console.WriteLine("\nOcurrio un error al recuperar los clientes.");
        }
    }
}
