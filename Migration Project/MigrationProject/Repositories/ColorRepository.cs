using MigrationProject.Data;
using MigrationProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MigrationProject.Repositories
{
    public class ColorRepository : Repository<int, Color>
    {
        public ColorRepository(MigrationContexts context) : base(context) {}

        public override async Task<Color?> Get(int key)
        {
            return await _context.Colors
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.ColorId == key);
        }

        public override async Task<IEnumerable<Color>> GetAll()
        {
            return await _context.Colors
                .Include(c => c.Products)
                .ToListAsync();
        }
    }
}
