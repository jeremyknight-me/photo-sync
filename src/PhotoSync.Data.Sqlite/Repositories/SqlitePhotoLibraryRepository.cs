﻿using PhotoSync.Domain.Abstractions;
using PhotoSync.Domain.Entities;
using PhotoSync.Domain.Operations;

namespace PhotoSync.Data.Sqlite.Repositories;

public sealed class SqlitePhotoLibraryRepository : IPhotoLibraryRepository
{
    private readonly PhotoSyncContextFactory contextFactory;
    private readonly IRefreshLibraryOperation refreshOperation;

    public SqlitePhotoLibraryRepository(
        PhotoSyncContextFactory dbContextFactory,
        IRefreshLibraryOperation refreshLibraryOperation)
    {
        this.contextFactory = dbContextFactory;
        this.refreshOperation = refreshLibraryOperation;
    }

    public PhotoLibrary Create(string filePath)
    {
        using var context = this.contextFactory.Make(filePath, true);
        if (context.PhotoLibraries.Any())
        {
            throw new InvalidOperationException("PhotoSync database already exists at this file path.");
        }

        var library = PhotoLibrary.Create(filePath);
        this.refreshOperation.Run(library);
        context.PhotoLibraries.Add(library);
        context.SaveChanges();
        return library;
    }


    public PhotoLibrary Open(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"PhotoSync database could not be found at the file path: {filePath}");
        }

        using var context = this.contextFactory.Make(filePath, false);
        var library = context.PhotoLibraries.SingleOrDefault()
            ?? throw new InvalidOperationException("PhotoSync database contains multiple photo libraries.");

        library.FilePath = filePath;
        this.refreshOperation.Run(library);
        context.SaveChanges();
        return library;
    }

    public void Save(PhotoLibrary library)
    {
        using var context = this.contextFactory.Make(library.FilePath);
        context.Update(library);
        context.SaveChanges();
    }
}
