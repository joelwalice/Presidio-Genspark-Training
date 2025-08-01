using MigrationProject.Data;
using MigrationProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MigrationProject.Repositories
{
    public class UserRepository : Repository<int, User>
    {
        public UserRepository(MigrationContexts context) : base(context)
        {
        }

        public override async Task<User?> Get(int key)
        {
            return await _context.Users
                .Include(u => u.News)
                .Include(u => u.Products)
                .FirstOrDefaultAsync(u => u.UserId == key);
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                .Include(u => u.News)
                .Include(u => u.Products)
                .ToListAsync();
        }
    }
}