using Microsoft.EntityFrameworkCore;
using PhotoSync.Data.Entities;

namespace PhotoSync.Data
{
    public class PhotoSyncContext : DbContext
    {
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
