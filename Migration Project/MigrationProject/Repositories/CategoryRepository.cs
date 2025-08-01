using MigrationProject.Data;
using MigrationProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MigrationProject.Repositories
{
    public class CategoryRepository : Repository<int, Category>
    {
        public CategoryRepository(MigrationContexts context) : base(context) {}

        public override async Task<Category?> Get(int key)
        {
            return await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == key);
        }

        public override async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();
        }
    }
}
