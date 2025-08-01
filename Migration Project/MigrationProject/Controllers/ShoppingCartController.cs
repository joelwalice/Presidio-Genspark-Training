using MigrationProject.DTOs;
using MigrationProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MigrationProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartService _shoppingCartService;

    public ShoppingCartController(IShoppingCartService shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<OrderResponseDto>> Checkout([FromBody] OrderRequestDto request)
    {
        var result = await _shoppingCartService.ProcessOrderAsync(request);
        return Ok(result);
    }
    [HttpPost("add")]
    public async Task<ActionResult> AddToCart([FromBody] CartItemDto request)
    {
        await _shoppingCartService.AddToCartAsync(request.ProductId, request.Quantity);
        return Ok();
    }
}