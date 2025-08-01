using MigrationProject.DTOs;

namespace MigrationProject.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderReadDto>> GetAllAsync();
    Task<OrderReadDto?> GetByIdAsync(int id);
    Task<byte[]?> ExportOrderListingPdfAsync();
}