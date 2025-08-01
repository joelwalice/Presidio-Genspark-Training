using MigrationProject.Data;
using MigrationProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MigrationProject.Repositories
{
    public class OrderDetailRepository : Repository<(int OrderId, int ProductId), OrderDetail>
    {
        public OrderDetailRepository(MigrationContexts context) : base(context) {}

        public override async Task<OrderDetail?> Get((int OrderId, int ProductId) key)
        {
            return await _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Product)
                .FirstOrDefaultAsync(od => od.OrderID == key.OrderId && od.ProductID == key.ProductId);
        }

        public override async Task<IEnumerable<OrderDetail>> GetAll()
        {
            return await _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Product)
                .ToListAsync();
        }
    }
}
