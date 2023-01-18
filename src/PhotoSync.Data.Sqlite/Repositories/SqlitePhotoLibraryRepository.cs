using System;
using System.IO;
using System.Linq;
using PhotoSync.Domain;
using PhotoSync.Domain.Contracts;
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

    public PhotoLibrary Create(string filePath, string sourceFolder)
    {
        using var context = this.contextFactory.Make(filePath, true);
        if (context.PhotoLibraries.Any())
        {
            throw new InvalidOperationException("PhotoSync database already exists at this file path.");
        }

        var library = PhotoLibrary.Create(filePath, sourceFolder);
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
        var library = context.PhotoLibraries.SingleOrDefault();
        if (library is null)
        {
            throw new InvalidOperationException("PhotoSync database contains multiple photo libraries.");
        }

        library.SetFilePath(filePath);
        this.refreshOperation.Run(library);
        context.SaveChanges();
        return library;
    }

    public void Save(string filePath, PhotoLibrary library)
    {
        using var context = this.contextFactory.Make(filePath);
        context.Update(library);
        context.SaveChanges();
    }
}
