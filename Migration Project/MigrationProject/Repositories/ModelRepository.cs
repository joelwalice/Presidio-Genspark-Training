using MigrationProject.Data;
using MigrationProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MigrationProject.Repositories
{
    public class ModelRepository : Repository<int, Model>
    {
        public ModelRepository(MigrationContexts context) : base(context) {}

        public override async Task<Model?> Get(int key)
        {
            return await _context.Models
                .Include(m => m.Products)
                .FirstOrDefaultAsync(m => m.ModelId == key);
        }

        public override async Task<IEnumerable<Model>> GetAll()
        {
            return await _context.Models
                .Include(m => m.Products)
                .ToListAsync();
        }
    }
}
