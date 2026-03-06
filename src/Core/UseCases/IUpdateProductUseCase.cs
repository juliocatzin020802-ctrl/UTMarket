using UTMarket.Core.Entities;

namespace UTMarket.Core.UseCases;

/// <summary>
/// Caso de uso para actualizar la informaciÃ³n de un producto existente.
/// </summary>
public interface IUpdateProductUseCase
{
    /// <summary>
    /// Actualiza los datos del producto proporcionado.
    /// </summary>
    /// <param name="product">Objeto de dominio con los nuevos datos.</param>
    /// <param name="ct">Token de cancelaciÃ³n.</param>
    Task ExecuteAsync(Product product, CancellationToken ct = default);
}
