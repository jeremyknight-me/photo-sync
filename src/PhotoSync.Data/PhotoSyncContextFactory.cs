using Microsoft.EntityFrameworkCore;

namespace PhotoSync.Data
{
    public static class PhotoSyncContextFactory
    {
        public static PhotoSyncContext Make(string filePath, bool migrate = false)
        {
            var options = new DbContextOptionsBuilder<PhotoSyncContext>()
                .UseSqlite($"Data Source={filePath}")
                .Options;
            var context = new PhotoSyncContext(options);
            if (migrate)
            {
                context.Database.Migrate();
            }            
            return context;
        }
    }
}
