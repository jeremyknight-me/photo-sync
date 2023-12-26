using PhotoSync.Domain.Entities;

namespace PhotoSync.Domain.Contracts;

public interface IPhotoLibraryRepository
{
    PhotoLibrary Create(string filePath, string sourceFolder);
    PhotoLibrary Open(string filePath);
    void Save(string filePath, PhotoLibrary library);
}
