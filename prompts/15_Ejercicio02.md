Prompts ejercicio 2

*ABSTRACCIÓN*

<role>
Actúa como un Arquitecto Senior  experto en Clean Architecture y patrones de diseño de aplicaciones empresariales. 
Tu objetivo es definir la abstracción para el "Sistema de Alertas de Inventario Crítico" del proyecto "UTMarket.csproj", asegurando un diseño desacoplado y orientado a casos de uso.
</role>

<context>
- Proyecto: UTMarket.csproj (Capa de Aplicación / Use Cases).
- Framework: .NET [[Version, e.g., 10.0]].
- Patrón: Use Case Pattern / Interactor (Capa central de lógica de aplicación).
- Enfoque: Abstracción pura para permitir múltiples implementaciones (Alertas por consola, Email, Push).
- Ubicación: "src/Application/Interfaces/UseCases/Inventory/".
</context>

<task>
Definir la interfaz 'ILowStockAlertUseCase.cs' que represente la lógica de negocio necesaria para identificar productos que han alcanzado niveles críticos de existencia.
</task>

<use_case_requirements>
1. Definición del Contrato:
   - El caso de uso debe ser capaz de procesar una solicitud de revisión de stock basada en un umbral (threshold) dinámico.
2. Firma del Método:
   - Debe retornar un flujo de datos asíncrono ('IAsyncEnumerable' o 'Task<IEnumerable>') para manejar colecciones de productos de forma eficiente.
3. Propósito:
   - Filtrar productos cuyo stock actual sea menor o igual al límite establecido por las reglas de negocio.
</use_case_requirements>

<coding_standards>
- Principio de Responsabilidad Única (SRP): La interfaz debe centrarse exclusivamente en la detección de stock crítico.
- Documentación Técnica: Incluir comentarios XML que expliquen la importancia del parámetro 'threshold' en la lógica de inventarios.
- Nombramiento: Seguir la convención 'I[Action]UseCase' para mantener la claridad semántica de la arquitectura.
</coding_standards>

<source_reference>
- Entidad Relacionada: Product { int Id, string Name, int Stock }.
- Namespace sugerido: UTMarket.csproj.Application.Interfaces.UseCases.
</source_reference>

<output_format>
Devuelve un documento Markdown que incluya:
1. El código fuente completo de la interfaz 'ILowStockAlertUseCase.cs'.
2. Una breve explicación de por qué este contrato pertenece a la capa de Aplicación y no a la de Infraestructura.
3. Nota técnica sobre el beneficio de usar interfaces de Casos de Uso para facilitar el Testing de Integración.
</output_format>

*LÓGICA DE APLICACIÓN*

<role>
Actúa como un Desarrollador Senior experto en patrones de diseño y optimización de flujos de datos asíncronos en .NET. 
Tu objetivo es implementar la lógica del caso de uso "Alerta de Inventario Crítico" para el proyecto "UTMarket.csproj", asegurando un procesamiento eficiente en memoria.
</role>

<context>
- Proyecto: UTMarket.csproj (Capa de Aplicación / Implementaciones).
- Framework: .NET [[Version, e.g., 10.0]].
- Patrón: Use Case Implementation (Interactor).
- Optimización: Streaming de datos con 'IAsyncEnumerable' para evitar la materialización de listas pesadas en RAM.
- Ubicación: "src/Application/UseCases/Inventory/".
</context>

<task>
Implementar la clase 'LowStockAlertUseCaseImpl.cs' que ejecute la lógica de filtrado de productos con stock crítico, consumiendo los datos desde la capa de persistencia.
</task>

<logic_requirements>
1. Firma del Método:
   - Debe implementar el método definido en 'ILowStockAlertUseCase'.
   - Parámetro: Un entero 'threshold' (umbral) que define el límite de stock crítico.
   - Retorno: 'IAsyncEnumerable<Product>'.
2. Funcionalidad:
   - El método debe iterar sobre la fuente de datos de productos de manera asíncrona.
   - Debe filtrar y devolver únicamente aquellos productos cuyo 'Stock' sea menor o igual (<=) al 'threshold' proporcionado.
3. Inyección de Dependencias:
   - Uso obligatorio de 'Primary Constructors' (C# 14) para inyectar el repositorio 'IProductRepository'.
</logic_requirements>

<coding_standards>
- Eficiencia: Utilizar la palabra clave 'yield return' dentro de un ciclo 'await foreach' o delegar directamente el flujo asíncrono desde el repositorio.
- Clean Code: Validar que el 'threshold' no sea un valor negativo antes de iniciar la consulta.
- Abstracción: La lógica no debe conocer detalles de la base de datos, solo interactuar con la interfaz del repositorio.
</coding_standards>

<source_reference>
- Interfaz a Implementar: ILowStockAlertUseCase.
- Dependencia Requerida: IProductRepository.GetAllAsync() (que idealmente retorne IAsyncEnumerable).
- Entidad: Product { int Id, string Name, int Stock }.
</source_reference>

<output_format>
Devuelve un documento Markdown que incluya:
1. El código fuente completo de la clase 'LowStockAlertUseCaseImpl.cs'.
2. Un ejemplo de cómo consumir este caso de uso desde un cliente (UI) usando 'await foreach'.
3. Nota técnica sobre por qué 'IAsyncEnumerable' es la mejor elección para sistemas de alertas de inventario frente a 'Task<List<Product>>'.
</output_format>

*INYECCIÓN DE DEPENDENCIAS/OPTIMIZACIÓN*

<role>
Actúa como un Ingeniero Senior experto en el ciclo de vida de aplicaciones .NET. 
Tu objetivo es configurar el contenedor de Inyección de Dependencias (DI) en el archivo 'Program.cs' para el proyecto "UTMarket.csproj", asegurando que el sistema de alertas de inventario esté correctamente orquestado y optimizado.
</role>

<context>
- Proyecto: UTMarket.csproj (Capa de Host / Entry Point).
- Framework: .NET 10 (Minimal APIs / Console Host).
- Patrón: Dependency Injection (Microsoft.Extensions.DependencyInjection).
- Optimización: Registro de servicios compatible con Trimming y bajo consumo de memoria.
- Ubicación: "root/Program.cs".
</context>

<task>
Realizar el registro de los servicios necesarios para el sistema de alertas de inventario, garantizando que las dependencias se resuelvan jerárquicamente de forma correcta.
</task>

<registration_requirements>
1. Registro de Repositorios: Configurar 'IProductRepository' con su implementación concreta 'SqlProductRepository'.
2. Registro de Casos de Uso: Configurar 'ILowStockAlertUseCase' con su implementación 'LowStockAlertUseCaseImpl'.
3. Ciclo de Vida (Lifetimes): 
   - Utilizar 'Scoped' para los repositorios si existe una conexión a base de datos compartida.
   - Utilizar 'Transient' o 'Scoped' para el caso de uso según las mejores prácticas de Clean Architecture.
</registration_requirements>

<optimization_standards>
- Native AOT Compliance: Evitar el uso de escaneo de ensamblados por reflexión (Assembly Scanning) para el registro de servicios; realizar registros explícitos para facilitar el Trimming.
- Asincronía: Asegurar que el punto de entrada de la aplicación soporte 'async Task Main'.
- Eficiencia: Minimizar la creación de servicios innecesarios en el arranque.
</optimization_standards>

<source_reference>
- Interfaz: ILowStockAlertUseCase.
- Implementación: LowStockAlertUseCaseImpl.
- Dependencia de Datos: IProductRepository.
</source_reference>

<output_format>
Devuelve un documento Markdown que incluya:
1. El fragmento de código completo para el archivo 'Program.cs' (o la sección de 'Service Collection').
2. Una breve guía sobre cómo verificar que el grafo de dependencias es válido al iniciar la aplicación.
3. Nota técnica sobre la diferencia de rendimiento entre registros explícitos frente al uso de librerías como Scrutor en aplicaciones optimizadas para Native AOT.
</output_format>