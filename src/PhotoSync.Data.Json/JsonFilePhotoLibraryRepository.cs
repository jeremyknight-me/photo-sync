using PhotoSync.Domain;
using PhotoSync.Domain.Contracts;
using PhotoSync.Domain.Operations;

namespace PhotoSync.Data.Json;

public sealed class JsonFilePhotoLibraryRepository : IPhotoLibraryRepository
{
    private readonly IRefreshLibraryOperation refreshOperation;

    public JsonFilePhotoLibraryRepository(IRefreshLibraryOperation refreshLibraryOperation)
    {
        this.refreshOperation = refreshLibraryOperation;
    }

    public PhotoLibrary Create(string libraryPath, string sourceFolder)
    {
        if (File.Exists(libraryPath))
        {
            throw new InvalidOperationException("PhotoSync database already exists at this file path.");
        }

        var library = PhotoLibrary.Create(libraryPath, sourceFolder);
        this.refreshOperation.Run(library);
        this.Save(libraryPath, library);
        return library;
    }

    public PhotoLibrary Open(string libraryPath)
    {
        if (!File.Exists(libraryPath))
        {
            throw new FileNotFoundException($"Photo library could not be found at given path: {libraryPath}");
        }

        var json = File.ReadAllText(libraryPath);
        var library = PhotoLibrarySerializer.Deserialize(json);
        this.refreshOperation.Run(library);
        this.Save(libraryPath, library);
        return library;
    }

    public void Save(string libraryPath, PhotoLibrary library)
    {
        var json = PhotoLibrarySerializer.Serialize(library);
        File.WriteAllText(libraryPath, json);
    }
}
