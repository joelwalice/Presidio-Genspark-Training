using Twitter.Models;
using Microsoft.EntityFrameworkCore;

namespace Twitter.Contexts
{
    public class TwitterContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("User ID=postgres;Password=1234;Host=localhost;Port=5432;Database=TwitterDB;");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<TweetHastag> TweetHastags { get; set; }
        public DbSet<Hastag> Hastags { get; set; }
        public DbSet<Like> Likes { get; set; }
    }
}