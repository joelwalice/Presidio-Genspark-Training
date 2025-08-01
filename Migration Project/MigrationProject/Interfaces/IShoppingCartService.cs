using MigrationProject.DTOs;

namespace MigrationProject.Interfaces;

public interface IShoppingCartService
{
    Task<OrderResponseDto> ProcessOrderAsync(OrderRequestDto request);
    Task AddToCartAsync(int productId, int quantity);
}