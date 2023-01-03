using PhotoSync.Domain;

namespace PhotoSync.Infrastructure;

internal interface IPhotoLibraryRepository
{
    PhotoLibrary Open(string path);
    void Save(PhotoLibrary library);
}
