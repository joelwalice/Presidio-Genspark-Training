using MigrationProject.Data;
using MigrationProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MigrationProject.Repositories
{
    public class ContactUsRepository : Repository<int, ContactUs>
    {
        public ContactUsRepository(MigrationContexts context) : base(context) {}

        public override async Task<ContactUs?> Get(int key)
        {
            return await _context.ContactUs.FindAsync(key);
        }

        public override async Task<IEnumerable<ContactUs>> GetAll()
        {
            return await _context.ContactUs.ToListAsync();
        }
    }
}
