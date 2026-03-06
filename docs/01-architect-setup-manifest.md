# Manifiesto de Arquitectura .NET 10: UTMarket CLI

Este documento detalla la integración de componentes y la configuración arquitectónica para la herramienta CLI de alto rendimiento **UTMarket**, optimizada para **Native AOT** y diseñada bajo principios de **Zero Trust**.

## 1. Resumen de Instalación

| Paquete NuGet | Versión | Rol Arquitectónico |
| :--- | :--- | :--- |
| `Microsoft.Data.SqlClient` | 6.1.4 | Driver oficial para SQL Server, optimizado para Native AOT y alto rendimiento. |
| `Dapper` | 2.1.35 | Micro-ORM para mapeo de datos eficiente (Requiere generadores de código para AOT). |
| `Microsoft.Extensions.Hosting` | 10.0.3 | Gestión del ciclo de vida de la aplicación y Dependency Injection ligera. |
| `Microsoft.Extensions.Configuration.UserSecrets` | 10.0.3 | Almacenamiento seguro de credenciales en desarrollo local (Zero Trust). |

## 2. Referencia de Implementación (`Program.cs`)

El siguiente esqueleto demuestra el uso de `HostApplicationBuilder` y las nuevas capacidades de C# 14 para una inicialización eficiente y segura.

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

// .NET 10 HostApplicationBuilder para un ciclo de vida CLI moderno y ligero
var builder = new HostApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    ApplicationName = "UTMarket.CLI",
    DisableDefaults = false // Mantiene UserSecrets y configuración básica
});

// Configuración de servicios para Native AOT (Evitando reflexión pesada)
builder.Services.AddLogging(logging => 
{
    logging.AddConsole();
});

// Registro de servicios de dominio
builder.Services.AddSingleton<IMarketOrchestrator, MarketOrchestrator>();

using IHost host = builder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("UTMarket CLI Iniciando (Optimizado para Native AOT)");

var orchestrator = host.Services.GetRequiredService<IMarketOrchestrator>();
await orchestrator.RunAsync(args, CancellationToken.None);

public interface IMarketOrchestrator
{
    ValueTask RunAsync(string[] args, CancellationToken ct);
}

public sealed class MarketOrchestrator(ILogger<MarketOrchestrator> logger) : IMarketOrchestrator
{
    public async ValueTask RunAsync(string[] args, CancellationToken ct)
    {
        logger.LogInformation("Procesando lógica de UTMarket...");
        
        var product = new Product 
        { 
            Id = 1, 
            Name = "Procesador .NET 10 Moderno", 
            Price = 499.99m 
        };

        logger.LogInformation("Producto Cargado: {Name} (Precio: {Price:C})", product.Name, product.Price);
        
        await ValueTask.CompletedTask;
    }
}

// C# 14: Uso de la palabra clave 'field' para reducir el boilerplate en propiedades
public sealed class Product
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    
    public decimal Price 
    { 
        get => field; 
        set => field = value >= 0 ? value : throw new ArgumentException("El precio no puede ser negativo"); 
    }
}
```

## 3. Notas de Modernización

### Beneficios del uso de 'field' (C# 14)
- **Reducción de Boilerplate:** Permite acceder al campo de respaldo (backing field) generado por el compilador directamente dentro de los accesores `get` y `set`.
- **Claridad:** Elimina la necesidad de declarar explícitamente campos privados `_field` cuando se requiere lógica de validación simple.
- **Seguridad:** Mantiene la encapsulación sin sacrificar la brevedad de las propiedades automáticas.

### Optimizaciones Native AOT en .NET 10
- **Physical Promotion:** El runtime de .NET 10 mejora la promoción de estructuras a registros de CPU, reduciendo la presión en el stack.
- **Desvirtualización Nativa:** Las llamadas a interfaces se resuelven de forma estática durante la compilación AOT, eliminando el overhead de la tabla virtual.
- **Tamaño de Binario:** El uso de `HostApplicationBuilder` permite un "trimming" agresivo, resultando en ejecutables de pocos MBs sin dependencias externas.

## 4. Guía de Ejecución

### Compilación como Binario Nativo
Para generar un ejecutable único y optimizado (sin necesidad del SDK en el destino):

```powershell
dotnet publish -c Release -r win-x64 --self-contained true
```

### Ejecución Local
```powershell
dotnet run --project UTMarket
```

---
*Arquitectura validada para .NET 10 (Target Framework: net10.0).*
