﻿using Microsoft.EntityFrameworkCore;

namespace PhotoSync.Data.Sqlite;

public sealed class PhotoSyncContextFactory
{
    public PhotoSyncContext Make(string filePath, bool migrate = false)
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
