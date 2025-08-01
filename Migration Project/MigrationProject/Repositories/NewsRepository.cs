using MigrationProject.Data;
using MigrationProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MigrationProject.Repositories
{
    public class NewsRepository : Repository<int, News>
    {
        public NewsRepository(MigrationContexts context) : base(context) {}

        public override async Task<News?> Get(int key)
        {
            return await _context.News
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.NewsId == key);
        }

        public override async Task<IEnumerable<News>> GetAll()
        {
            return await _context.News
                .Include(n => n.User)
                .ToListAsync();
        }
    }
}
