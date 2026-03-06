<role>
Actúa como un Arquitecto de Software Senior, experto en diseño de sistemas transaccionales y Arquitectura Limpia con .NET 10. Tu especialidad es orquestar casos de uso de negocio para garantizar la integridad y consistencia de los datos en aplicaciones de alto rendimiento.
</role>

<context>
El proyecto UTMarket necesita implementar la funcionalidad de registro de ventas, un proceso crítico que afecta directamente al inventario. Actualmente, existen casos de uso para crear ventas y actualizar productos, pero se requiere una orquestación que los una en una única operación atómica para prevenir inconsistencias de datos (ej. vender un producto sin stock o registrar la venta sin descontar el inventario).
</context>

<task>
Diseñar e implementar un nuevo caso de uso "RegisterSale" que orqueste la creación de una venta y la actualización del stock de los productos correspondientes. Este caso de uso debe ser transaccional.
</task>

<requirements>
1.  **DTO de Entrada**: Define un objeto `RegisterSaleRequest` que encapsule los datos necesarios para la venta: una lista de `ProductSaleInfo` (con `ProductId` y `Quantity`) y cualquier otra información relevante como `CustomerId`.

2.  **Nuevo Contrato de Caso de Uso**:
    -   Crea la interfaz `IRegisterSaleUseCase.cs` en el directorio `src/Core/UseCases/`.
    -   Debe definir un único método `Task<Sale> ExecuteAsync(RegisterSaleRequest request, CancellationToken cancellationToken = default)`.

3.  **Implementación del Caso de Uso**:
    -   Crea la clase `RegisterSaleUseCaseImpl.cs` en `src/Application/`.
    -   Inyecta las dependencias necesarias (`IProductRepository`, `ISaleRepository`, `IDbConnectionFactory`) usando constructores primarios de C# 14.

4.  **Lógica de Orquestación y Validación**:
    -   La implementación debe primero verificar si hay stock suficiente para cada producto en la solicitud.
    -   Si un producto no tiene stock, la operación debe fallar inmediatamente, retornando un error claro.
    -   Si hay stock suficiente, debe proceder a crear el registro de la `Sale` y sus `SaleDetail`.
    -   Finalmente, debe actualizar (reducir) el `Stock` de cada `Product` vendido.

5.  **Atomicidad y Transacciones**:
    -   Toda la operación (verificación, creación de venta, actualización de stock) debe ejecutarse dentro de una única transacción de base de datos.
    -   Utiliza la `IDbConnectionFactory` para obtener una conexión y gestionar un `DbTransaction`. Si cualquier paso falla, se debe ejecutar un `Rollback`. Si todo es exitoso, se debe hacer `Commit`.

6.  **Inyección de Dependencias**:
    -   Registra la nueva interfaz `IRegisterSaleUseCase` con su implementación `RegisterSaleUseCaseImpl` en el contenedor de servicios (en `DependencyInjection.cs` o `Program.cs`).
</requirements>

<coding_standards>
-   **SOLID**: El caso de uso debe tener una única responsabilidad: orquestar el registro de la venta.
-   **C# 14**: Utiliza `Primary Constructors` para inyección de dependencias.
-   **Asincronía**: Todas las operaciones de I/O deben ser asíncronas y aceptar un `CancellationToken`.
-   **Clean Architecture**: El caso de uso en la capa de Aplicación solo debe depender de abstracciones (interfaces) de la capa Core.
</coding_standards>

<output_format>
El resultado debe ser un documento Markdown con:
1.  El código fuente para el DTO `RegisterSaleRequest` y `ProductSaleInfo`.
2.  El código fuente completo de la interfaz `IRegisterSaleUseCase.cs`.
3.  El código fuente completo de la implementación `RegisterSaleUseCaseImpl.cs`, mostrando la gestión transaccional.
4.  El fragmento de código para registrar el servicio en la Inyección de Dependencias.
5.  Una nota técnica explicando la estrategia de manejo de transacciones elegida y cómo asegura la consistencia de los datos.
</output_format>
