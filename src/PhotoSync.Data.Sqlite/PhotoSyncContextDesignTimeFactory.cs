using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PhotoSync.Data.Sqlite;

public sealed class PhotoSyncContextDesignTimeFactory : IDesignTimeDbContextFactory<PhotoSyncContext>
{
    public PhotoSyncContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<PhotoSyncContext>();
        builder.UseSqlite("Data Source=foo.dat");
        return new PhotoSyncContext(builder.Options);
    }
}
