using Microsoft.EntityFrameworkCore;
using PhotoSync.Entities;

namespace PhotoSync.Common;

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

    public DbSet<ExcludeFolder> ExcludeFolders { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Settings> Settings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // todo: implement correct handling of default providers
            // invalid data source used to allow ef migrations to work
            optionsBuilder.UseSqlite("Data Source=foo.dat");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = this.GetType().Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly, x => x.Namespace == "PhotoSync.Entities");
    }
}
