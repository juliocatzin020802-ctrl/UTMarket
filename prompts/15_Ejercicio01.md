Prompts ejercicio 1

*MODELO DE DOMINIO*

<role>
Actúa como un Senior  Arquitecto de Software experto
Tu objetivo es implementar el "Modelo de Dominio de Clientes" para el proyecto "UTMarket.csproj", aplicando las últimas características de C# 14 para validación de datos.
</role>

<context>
- Proyecto: UTMarket.csproj (Capa de Dominio / Entities).
- Lenguaje: C# 14 (.NET 10).
- Característica Clave: Uso de la palabra clave 'field' para lógica de backing fields automáticos.
- Ubicación: "src/Domain/Entities/".
</context>

<task>
Generar la clase de dominio 'Customer.cs' que represente a un cliente del sistema, integrando reglas de validación de negocio directamente en la definición de sus propiedades.
</task>

<domain_requirements>
1. Propiedades Obligatorias:
   - 'CustomerId' (Guid o int según estándar del proyecto).
   - 'FullName' (string).
   - 'Email' (string).
   - 'IsActive' (bool).
2. Regla de Negocio (Email):
   - El setter de la propiedad 'Email' debe validar que el valor contenga un formato de correo electrónico válido (presencia de '@' y '.').
   - Implementar esta validación utilizando la palabra clave 'field' de C# 14 para evitar la declaración manual de campos privados.
</domain_requirements>

<coding_standards>
- Modernidad: Utilizar Primary Constructors si se requiere inicialización obligatoria.
- Encapsulamiento: Asegurar que el estado del objeto sea consistente desde su creación.
- Limpieza: Mantener la entidad como un POCO (Plain Old CLR Object) sin dependencias externas de frameworks de validación (usar lógica nativa).
</coding_standards>

<source_reference>
- Namespace sugerido: UTMarket.csproj.Domain.Entities.
- Referencia de C# 14: La palabra clave 'field' permite acceder al campo de respaldo generado por el compilador dentro de los accessors (get/set).
</source_reference>

<output_format>
Devuelve un documento Markdown que incluya:
1. El código fuente completo de la clase 'Customer.cs'.
2. Un ejemplo de uso donde se intente asignar un email inválido para demostrar la validación.
3. Nota técnica sobre las ventajas de usar la palabra clave 'field' frente a las propiedades automáticas tradicionales con campos privados manuales.
</output_format>

*CONTRATO DE PERSISTENCIA*

<role>
Actúa como un Arquitecto Senior   experto en Patrones de Diseño y Persistencia de Datos. 
Tu objetivo es definir el "Contrato de Persistencia de Clientes" para el proyecto "UTMarket.csproj", asegurando un desacoplamiento total entre la lógica de negocio y la base de datos.
</role>

<context>
- Proyecto: UTMarket.csproj (Capa Core / Interfaces).
- Framework: .NET [[Version, e.g., 10.0]].
- Patrón: Repository Pattern (Abstracción de acceso a datos).
- Ubicación: "src/Core/Interfaces/Repositories/".
</context>

<task>
Definir la interfaz 'ICustomerRepository.cs' que servirá como el contrato obligatorio para cualquier implementación de persistencia (SQL Server, MongoDB, InMemory, etc.) relacionada con la entidad 'Customer'.
</task>

<technical_requirements>
1. Métodos Obligatorios:
   - 'GetByEmailAsync(string email)': Debe permitir la búsqueda de un cliente único mediante su dirección de correo.
   - 'AddAsync(Customer customer)': Debe permitir el registro de una nueva entidad 'Customer' en el almacén de datos.
2. Firma Asíncrona: Todos los métodos deben retornar 'Task' o 'ValueTask' para soportar operaciones de I/O no bloqueantes.
3. Tipado: Asegurar el uso correcto de la entidad 'Customer' definida en el bloque de dominio.
</technical_requirements>

<coding_standards>
- Abstracción: La interfaz no debe contener detalles de implementación (nada de SQL, strings de conexión o tipos específicos de frameworks).
- Documentación: Incluir comentarios XML (///) para describir los parámetros y valores de retorno de cada método.
- Nombramiento: Seguir las convenciones de C# para interfaces (prefijo 'I') y métodos asíncronos (sufijo 'Async').
</coding_standards>

<source_reference>
- Entidad Relacionada: Customer { CustomerId, FullName, Email, IsActive }.
- Namespace sugerido: UTMarket.csproj.Core.Interfaces.
</source_reference>

<output_format>
Devuelve un documento Markdown que incluya:
1. El código fuente completo de la interfaz 'ICustomerRepository.cs'.
2. Un ejemplo breve de cómo se inyectaría esta interfaz en un servicio de aplicación (Constructor Injection).
3. Nota técnica sobre por qué definir interfaces en la capa Core es fundamental para la Testabilidad (Unit Testing) y el principio de Inversión de Dependencias (DIP).
</output_format>


*IMPLEMENTACIÓN DE INFRAESTRUCTURA/ESTÁNDARES*

<role>
Actúa como un Ingeniero Senior en Base de Datos y experto en optimización de .NET 10. 
Tu objetivo es implementar la capa de persistencia para el módulo de Clientes en el proyecto "UTMarket.csproj", garantizando compatibilidad con Native AOT y máxima eficiencia en memoria.
</role>

<context>
- Proyecto: UTMarket.csproj(Capa de Infraestructura / Data Access).
- Stack: .NET 10, C# 14, SQL Server 2022.
- Técnica: Mapeo manual mediante 'SqlDataReader' (Sin ORMs dinámicos ni reflexión).
- Optimización: Native AOT Readiness (Trimming friendly).
- Ubicación: "src/Infrastructure/Repositories/".
</context>

<task>
1. Definir la interfaz 'ICustomerRepository.cs' en la capa Core.
2. Implementar la clase concreta 'SqlCustomerRepository.cs' utilizando ADO.NET puro.
</task>

<technical_requirements>
1. Interfaz (ICustomerRepository):
   - 'GetByEmailAsync(string email)': Retorna 'Task<Customer?>'.
   - 'AddAsync(Customer customer)': Retorna 'Task<int>' (ID generado).
2. Implementación (SqlCustomerRepository):
   - Uso obligatorio de 'Primary Constructors' para inyectar la cadena de conexión o el 'DbConnection'.
   - Mapeo Manual: Extraer cada columna del 'SqlDataReader' manualmente para evitar el uso de reflexión.
   - Manejo de Recursos: Uso estricto de la declaración 'using' para asegurar el cierre de conexiones y comandos.
</technical_requirements>

<coding_standards>
- Native AOT: Las clases deben ser simples y el código de mapeo debe ser estático o explícito para que el compilador pueda realizar un trimming agresivo.
- C# 14: Aplicar 'Primary Constructors' en la definición de la clase del repositorio.
- Seguridad: Uso de parámetros de SQL ('SqlParameter') para prevenir inyecciones SQL.
</coding_standards>

<source_reference>
- Entidad: Customer { CustomerId, FullName, Email, IsActive }.
- Tabla SQL: [dbo].[Customers] (CustomerId INT PK, FullName NVARCHAR(200), Email NVARCHAR(255), IsActive BIT).
</source_reference>

<output_format>
Devuelve un documento Markdown que incluya:
1. El código de la interfaz 'ICustomerRepository.cs'.
2. El código completo de 'SqlCustomerRepository.cs' con la lógica de SqlDataReader.
3. Nota técnica sobre por qué el mapeo manual es la opción recomendada para aplicaciones que buscan el menor 'Startup Time' posible en entornos Native AOT.
</output_format>
