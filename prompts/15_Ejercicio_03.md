Prompts ejercicio 3

*INTERFAZ DE USUARIO*
<role>
Actúa como un Desarrollador Senior especializado en Interfaces de Consola (CLI) y UX para herramientas técnicas. 
Tu objetivo es diseñar e implementar el "Módulo de Interacción de Historial de Ventas" para el proyecto "UTMarket.csproj", asegurando una experiencia de usuario fluida y libre de errores de entrada.
</role>

<context>
- Proyecto: UTMarket.csproj (Capa de Presentación / UI de Consola).
- Framework: .NET 10.
- Objetivo UX: Proporcionar una forma intuitiva de filtrar datos históricos sin romper el flujo de ejecución.
- Ubicación: "src/Presentation/Console/Components/".
</context>

<task>
Implementar la nueva opción de menú "Consultar ventas por fecha" y la lógica de captura de parámetros necesaria para invocar los servicios de reporte.
</task>

<ui_ux_requirements>
1. Integración de Menú:
   - Añadir una entrada clara al menú principal: "[X] Consultar ventas por fecha".
2. Flujo de Captura:
   - El sistema debe guiar al usuario para ingresar la "Fecha de Inicio" y la "Fecha de Fin".
3. Diseño de Interfaz:
   - Utilizar separadores visuales (líneas, encabezados) para diferenciar la sección de consulta del resto de la consola.
</ui_ux_requirements>

<technical_standards>
- Robustez: La interfaz debe ser capaz de volver al menú principal si el usuario decide cancelar la operación.
- Feedback: Mostrar mensajes claros de "Cargando..." o "Buscando..." mientras se procesa la solicitud asíncrona.
- Desacoplamiento: La clase de UI no debe contener lógica de negocio; solo debe recolectar datos y llamar al caso de uso correspondiente.
</technical_standards>

<source_reference>
- Punto de integración: MenuHandler.cs o Program.cs.
- Caso de uso a invocar (vía DI): IFetchSalesByFilterUseCase.
</source_reference>

<output_format>
Devuelve un documento Markdown que incluya:
1. El código del componente de UI encargado de la opción de consulta.
2. El snippet de integración en el bucle principal (Main Loop) de la consola.
3. Una breve guía sobre cómo mejorar la legibilidad de fechas en la consola (ej. usando máscaras de entrada o ejemplos de formato).
</output_format>


*CAPTURA DE DATOS*
<role>
Actúa como un Desarrollador Senior experto en validación de datos y manejo de excepciones en C#. 
Tu objetivo es implementar la "Lógica de Captura y Validación de Fechas" para el reporte de ventas del proyecto "UTMarket.csproj", asegurando una entrada de datos robusta y a prueba de errores.
</role>

<context>
- Proyecto: UTMarket.csproj (Capa de Presentación / Input Handling).
- Framework: .NET 10.
- Técnica: Validación defensiva mediante 'DateTime.TryParse'.
- UX: Ciclos de reintento (Retries) hasta que el usuario proporcione una fecha válida.
- Ubicación: "src/Presentation/Console/Utils/InputHelpers.cs".
</context>

<task>
Implementar un componente o método que capture el rango de fechas solicitado, validando rigurosamente el formato de entrada y la coherencia lógica (Fecha Fin >= Fecha Inicio).
</task>

<validation_requirements>
1. Captura Iterativa:
   - El sistema debe solicitar "Fecha de Inicio" y "Fecha de Fin" por separado.
   - Si el formato es inválido, debe mostrar un error y volver a solicitar la fecha específica sin reiniciar todo el proceso.
2. Validación Técnica:
   - Uso obligatorio de 'DateTime.TryParse' o 'DateTime.TryParseExact' (con formato 'yyyy-MM-dd' o local) para evitar excepciones de sistema.
3. Validación de Negocio:
   - Comprobar que la 'Fecha de Fin' no sea cronológicamente anterior a la 'Fecha de Inicio'.
   - Opcional: Impedir fechas futuras si el reporte es estrictamente histórico.
</validation_requirements>

<coding_standards>
- Claridad: Proporcionar al usuario un ejemplo del formato esperado (ej. "dd/mm/aaaa").
- Robustez: Manejar valores nulos o entradas vacías asignando valores por defecto o solicitando re-entrada.
- Limpieza: El código de validación debe ser reutilizable para otros módulos que requieran rangos de fechas.
</coding_standards>

<source_reference>
- Objeto de salida: SaleFilter { DateTime StartDate, DateTime EndDate }.
- Cultura: Usar 'CultureInfo.InvariantCulture' o la cultura local configurada en el proyecto.
</source_reference>

<output_format>
Devuelve un documento Markdown que incluya:
1. El código de la clase o método 'DateInputHandler'.
2. El flujo lógico (diagrama o lista) de cómo se maneja el error de validación.
3. Nota técnica sobre por qué 'DateTime.TryParse' es preferible sobre bloques 'try-catch' en términos de rendimiento y legibilidad.
</output_format>

*ORQUESTACION*
<role>
Actúa como un Desarrollador Senior experto en patrones de integración y orquestación de servicios en .NET. 
Tu objetivo es implementar la "Capa de Orquestación de Consultas" para el historial de ventas del proyecto "UTMarket.csproj", asegurando un flujo de datos limpio entre la UI y el Caso de Uso.
</role>

<context>
- Proyecto: UTMarket.csproj (Capa de Presentación / Orchestration).
- Framework: .NET 10.
- Patrón: Mediación de servicios (Separación de UI y Lógica de Aplicación).
- Objetivo: Transformar los inputs del usuario en una solicitud de negocio válida (SaleFilter).
- Ubicación: "src/Presentation/Console/Orchestrators/".
</context>

<task>
Implementar la lógica de orquestación que recolecte el rango de fechas validado, construya el objeto de filtro y ejecute la llamada asíncrona al caso de uso 'IFetchSalesByFilterUseCase'.
</task>

<orchestration_requirements>
1. Construcción de DTO:
   - Crear una instancia del objeto 'SaleFilter' utilizando la 'Fecha de Inicio' y 'Fecha de Fin' capturadas previamente.
2. Invocación de Negocio:
   - Consumir la interfaz 'IFetchSalesByFilterUseCase' inyectada mediante el contenedor de dependencias.
3. Manejo de Flujo Asíncrono:
   - La llamada debe ser 'awaitable' para evitar el bloqueo del hilo principal de la consola.
   - Gestionar el flujo de datos resultante (IAsyncEnumerable o IEnumerable) para preparar la visualización.
</orchestration_requirements>

<technical_standards>
- Desacoplamiento: La UI no debe conocer la implementación del caso de uso, solo su interfaz.
- Inyección de Dependencias: Asegurar que el orquestador reciba 'IFetchSalesByFilterUseCase' a través de su constructor (usando Primary Constructors de C# 14).
- Gestión de Estados: Manejar el estado de "Sin resultados" si el caso de uso devuelve una colección vacía para el rango de fechas dado.
</technical_standards>

<source_reference>
- Input: DateTime startDate, DateTime endDate.
- Contrato: IFetchSalesByFilterUseCase.ExecuteAsync(SaleFilter filter).
- Objeto de Filtro: SaleFilter { DateTime StartDate, DateTime EndDate }.
</source_reference>

<output_format>
Devuelve un documento Markdown que incluya:
1. El código del método de orquestación (ej. 'SalesHistoryOrchestrator').
2. La definición del objeto 'SaleFilter'.
3. Una breve explicación de por qué enviar un objeto 'Filter' es más escalable que enviar múltiples parámetros sueltos al caso de uso.
</output_format>

*VISUALIZACIÓN*

<role>
Actúa como un Diseñador para aplicaciones de terminal (CLI) y experto en formateo de datos en C#. 
Tu objetivo es implementar el "Módulo de Visualización de Historial de Ventas" para el proyecto "UTMarket.csproj", asegurando que la información sea clara, profesional y fácil de leer para el usuario final.
</role>

<context>
- Proyecto: UTMarket.csproj (Capa de Presentación / Report View).
- Framework: .NET 10.
- Formato: Tabla alineada en consola (ASCII Table o similar).
- Eficiencia: Renderizado fila por fila (ideal para IAsyncEnumerable) para evitar bloqueos visuales.
- Ubicación: "src/Presentation/Console/Views/".
</context>

<task>
Implementar la lógica de renderizado que tome la colección de ventas filtradas y las muestre en una tabla formateada en la consola del sistema.
</task>

<visualization_requirements>
1. Estructura de la Tabla:
   - Encabezados obligatorios: [Folio], [Fecha] y [Monto Total].
2. Formato de Datos:
   - Folio: Alineado a la izquierda.
   - Fecha: Formato corto legible (ej. dd/MM/yyyy).
   - Monto Total: Formato de moneda local (C2) y alineado a la derecha.
3. Resumen Final:
   - Al final de la tabla, mostrar el "Gran Total" sumando los montos de todas las ventas listadas.
   - Mostrar el conteo total de registros encontrados.
</visualization_requirements>

<technical_standards>
- UI Limpia: Implementar un ancho de columna fijo o dinámico para que los datos no se encimen.
- Manejo de Vacíos: Si no hay resultados, mostrar un mensaje amigable: "No se encontraron ventas en el rango de fechas seleccionado."
- Rendimiento: Si los datos provienen de un 'IAsyncEnumerable', la tabla debe poder empezar a dibujarse conforme llegan los datos o esperar a la carga completa según el diseño visual.
</technical_standards>

<source_reference>
- Entidad de Entrada: Sale { string Folio, DateTime CreatedAt, decimal TotalAmount }.
- Herramienta: Puede usar 'Console.WriteLine' con interpolación y alineación (ej. {0, -15}) o una librería ligera.
</source_reference>

<output_format>
Devuelve un documento Markdown que incluya:
1. El código de la clase 'SalesTableView'.
2. Un ejemplo visual (en bloque de texto) de cómo se verá la tabla final en la consola.
3. Nota técnica sobre el uso de la alineación en 'String Interpolation' (ej. padding) para crear tablas sin dependencias externas.
</output_format>

*RESTRICCIÓN DE APERTURA*

<role>
Actúa como un Senior Systems Architect experto en Dependency Injection (DI) y gestión de recursos en .NET. 
Tu objetivo es implementar el "Contenedor de Ejecución Aislada" para el reporte de ventas, asegurando que cada consulta se ejecute en su propio contexto de servicios (Scope).
</role>

<context>
- Proyecto: UTMarket.csproj (Capa de Infraestructura de Ejecución / Host).
- Framework: .NET 10.
- Patrón: Service Locator (controlado) mediante 'IServiceScopeFactory'.
- Restricción: Aplicación de consola con servicios 'Scoped' (como DbContexts o Repositorios).
- Ubicación: "src/Presentation/Console/Infrastructure/".
</context>

<task>
Implementar la lógica de resolución de servicios que cree un 'IServiceScope' manual para la ejecución del caso de uso de historial de ventas, garantizando la liberación de recursos al finalizar.
</task>

<architectural_requirements>
1. Creación de Scope:
   - Inyectar 'IServiceScopeFactory' en la clase principal o manejador de menús.
   - Utilizar el bloque 'using' para crear un nuevo alcance con 'factory.CreateScope()'.
2. Resolución de Dependencias:
   - Obtener la instancia de 'IFetchSalesByFilterUseCase' directamente desde el 'scope.ServiceProvider'.
3. Ciclo de Vida:
   - Asegurar que todos los servicios inyectados dentro de ese proceso (Repositorios, Handlers) se destruyan (Dispose) automáticamente al salir del bloque 'using'.
</architectural_requirements>

<technical_standards>
- Seguridad de Memoria: Evitar el "Captive Dependency" (dependencias cautivas) asegurando que los servicios de corta duración no vivan para siempre en el Root Provider.
- Modernidad: Utilizar la sintaxis de 'using' simplificada de C# si es posible.
- Robustez: El scope debe cerrarse incluso si ocurre una excepción durante la ejecución del caso de uso.
</technical_standards>

<source_reference>
- Inyección base: IServiceProvider o IServiceScopeFactory.
- Interfaz a resolver: IFetchSalesByFilterUseCase.
</source_reference>

<output_format>
Devuelve un documento Markdown que incluya:
1. El código de la clase o método encargado de gestionar el Scope de ejecución.
2. Un diagrama lógico (en texto o descripción) del flujo: Request -> Create Scope -> Resolve -> Execute -> Dispose.
3. Nota técnica sobre por qué resolver servicios desde el Root Provider en una aplicación de consola puede causar fugas de memoria (Memory Leaks).
</output_format>