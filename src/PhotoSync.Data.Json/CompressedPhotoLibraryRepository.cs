using System.IO.Compression;
using PhotoSync.Domain.Contracts;
using PhotoSync.Domain.Entities;
using PhotoSync.Domain.Operations;

namespace PhotoSync.Data.Json;

public sealed class CompressedPhotoLibraryRepository : IPhotoLibraryRepository
{
    private readonly IRefreshLibraryOperation refreshOperation;

    public CompressedPhotoLibraryRepository(IRefreshLibraryOperation refreshLibraryOperation)
    {
        this.refreshOperation = refreshLibraryOperation;
    }

    public PhotoLibrary Create(string libraryPath, string sourceFolder)
    {
        if (File.Exists(libraryPath))
        {
            throw new InvalidOperationException($"PhotoSync database already exists at file path: {libraryPath}");
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

        using var zipStream = new GZipStream(File.OpenRead(libraryPath), CompressionMode.Decompress);
        using var reader = new StreamReader(zipStream);
        var json = reader.ReadToEnd();
        reader.Close();
        var library = PhotoLibrarySerializer.Deserialize(json);
        library.SetFilePath(libraryPath);
        this.refreshOperation.Run(library);
        this.Save(libraryPath, library);
        return library;
    }

    public void Save(string libraryPath, PhotoLibrary library)
    {
        var json = PhotoLibrarySerializer.Serialize(library);
        using var zipStream = new GZipStream(File.OpenWrite(libraryPath), CompressionMode.Compress);
        using var sw = new StreamWriter(zipStream);
        sw.Write(json);
        sw.Close();
    }
}
