using Microsoft.EntityFrameworkCore;
using PhotoSync.Domain.Entities;

namespace PhotoSync.Data.Sqlite;

public sealed class PhotoSyncContext : DbContext
{
    public PhotoSyncContext()
    {
    }

    public PhotoSyncContext(DbContextOptions<PhotoSyncContext> options)
        : base(options)
    {
    }

    public DbSet<PhotoLibrary> PhotoLibraries { get; set; }
    public DbSet<ExcludedFolder> ExcludedFolders { get; set; }
    public DbSet<Photo> Photos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(PhotoSyncContext).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly, x => x.Namespace == "PhotoSync.Data.Sqlite.Configuration");
    }
}
