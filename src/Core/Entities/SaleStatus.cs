namespace UTMarket.Core.Entities;

/// <summary>
/// Represents the lifecycle status of a sale, aligned with the database schema.
/// Based on prompt 01-sql-server-architect.md (1: Pendiente, 2: Completada, 3: Cancelada)
/// </summary>
public enum SaleStatus
{
    Pending = 1,
    Completed = 2,
    Cancelled = 3
}
