using System.IO;
using PhotoSync.Domain;

namespace PhotoSync.Infrastructure;

internal sealed class JsonFilePhotoLibraryRepository : IPhotoLibraryRepository
{
    public PhotoLibrary Open(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Photo library could not be found at given path.");
        }

        var json = File.ReadAllText(path);
        var library = PhotoLibrarySerializer.Deserialize(json);
        return library;
    }

    public void Save(PhotoLibrary library)
    {
        var json = PhotoLibrarySerializer.Serialize(library);
        File.WriteAllText(library.LibraryPath, json);
    }
}
