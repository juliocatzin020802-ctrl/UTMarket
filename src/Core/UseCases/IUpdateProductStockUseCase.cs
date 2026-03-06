namespace UTMarket.Core.UseCases;

/// <summary>
/// Caso de uso especÃ­fico para actualizar el stock de un producto.
/// Sigue SRP al separar esta operaciÃ³n atÃ³mica de la actualizaciÃ³n general.
/// </summary>
public interface IUpdateProductStockUseCase
{
    /// <summary>
    /// Actualiza de forma atÃ³mica el inventario de un producto.
    /// </summary>
    /// <param name="productId">ID del producto.</param>
    /// <param name="newStock">Nueva cantidad disponible.</param>
    /// <param name="ct">Token de cancelaciÃ³n.</param>
    Task ExecuteAsync(int productId, int newStock, CancellationToken ct = default);
}
