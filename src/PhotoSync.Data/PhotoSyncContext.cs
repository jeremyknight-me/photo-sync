using Microsoft.EntityFrameworkCore;
using PhotoSync.Data.Entities;

namespace PhotoSync.Data
{
    public class PhotoSyncContext : DbContext
    {
        public PhotoSyncContext()
        {
            // required for ef migrations to work with OnConfiguring override
        }

        public PhotoSyncContext(DbContextOptions<PhotoSyncContext> options)
            : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Settings> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // invalid data source used to allow ef migrations to work
                optionsBuilder.UseSqlite("Data Source=foo.dat");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = this.GetType().Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly, x => x.Namespace == "PhotoSync.Data.Entities");
        }
    }
}
