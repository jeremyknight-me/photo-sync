using PhotoSync.Domain.Entities;

namespace PhotoSync.Domain.Abstractions;

public interface IPhotoLibraryRepository
{
    PhotoLibrary Create(string filePath);
    PhotoLibrary Open(string filePath);
    void Save(PhotoLibrary library);
}
