using System.IO;
using System.IO.Compression;
using PhotoSync.Domain;

namespace PhotoSync.Infrastructure;

internal sealed class CompressedPhotoLibraryRepository : IPhotoLibraryRepository
{
    public PhotoLibrary Open(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Photo library could not be found at given path.");
        }

        using var zipStream = new GZipStream(File.OpenRead(path), CompressionMode.Decompress);
        using var reader = new StreamReader(zipStream);
        var json = reader.ReadToEnd();
        reader.Close();
        var library = PhotoLibrarySerializer.Deserialize(json);
        return library;
    }

    public void Save(PhotoLibrary library)
    {
        var json = PhotoLibrarySerializer.Serialize(library);
        using var zipStream = new GZipStream(File.OpenWrite(library.LibraryPath), CompressionMode.Compress);
        using var sw = new StreamWriter(zipStream);
        sw.Write(json);
        sw.Close();
    }
}
