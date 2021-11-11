using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Space.ImdbWatchList.Data.Entities;

namespace Space.ImdbWatchList.Data
{
    public class ImdbWatchListDbContext : DbContext
    {
        public ImdbWatchListDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ImdbWatchListDbContext).Assembly);
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WatchList> WatchLists { get; set; }

    }
}