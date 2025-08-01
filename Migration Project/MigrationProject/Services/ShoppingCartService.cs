using MigrationProject.Data;
using MigrationProject.DTOs;
using MigrationProject.Interfaces;
using MigrationProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MigrationProject.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly MigrationContexts _context;

    public ShoppingCartService(MigrationContexts context)
    {
        _context = context;
    }

    public async Task<OrderResponseDto> ProcessOrderAsync(OrderRequestDto request)
    {
        var user = await _context.Users.FindAsync(request.UserId);
        if (user == null)
            throw new Exception("User not found");

        var productIds = request.Items.Select(i => i.ProductId).ToList();
        var products = await _context.Products
            .Where(p => productIds.Contains(p.ProductId))
            .ToListAsync();

        if (products.Count != productIds.Count)
            throw new Exception("One or more products not found");

        var order = new Order
        {
            OrderName = $"Order_{DateTime.UtcNow.Ticks}",
            CustomerName = user.Username,
            CustomerPhone = request.CustomerPhone,
            CustomerEmail = request.CustomerEmail,
            CustomerAddress = request.CustomerAddress,
            OrderDate = DateTime.UtcNow,
            PaymentType = "Cash",
            Status = "Processing"
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        foreach (var item in request.Items)
        {
            var product = products.First(p => p.ProductId == item.ProductId);
            var orderDetail = new OrderDetail
            {
                OrderID = order.OrderID,
                ProductID = product.ProductId,
                Quantity = item.Quantity,
                Price = product.Price ?? 0
            };
            _context.OrderDetails.Add(orderDetail);
        }

        await _context.SaveChangesAsync();

        return new OrderResponseDto
        {
            OrderId = order.OrderID,
            Status = order.Status
        };
    }
    public async Task AddToCartAsync(int productId, int quantity)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            throw new Exception("Product not found");
        var cartItem = new CartItem
        {
            ProductId = productId,
            Quantity = quantity
        };

        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();
    }
}
