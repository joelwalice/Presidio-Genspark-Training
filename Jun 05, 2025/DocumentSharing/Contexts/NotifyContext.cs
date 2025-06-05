using Microsoft.EntityFrameworkCore;
using DocumentSharing.Models;

namespace DocumentSharing.Contexts
{
    public class NotifyContext : DbContext
    {
        public NotifyContext(DbContextOptions<NotifyContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
    }
}