using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using PhotoSync.Domain;

namespace PhotoSync.Infrastructure;

internal interface IRefreshLibraryCommand
{
    void Run(PhotoLibrary library);
}

internal sealed class RefreshLibraryCommand : IRefreshLibraryCommand
{
    private readonly IGetPhotosQuery photosQuery;

    public RefreshLibraryCommand(IGetPhotosQuery getPhotosQuery)
    {
        this.photosQuery = getPhotosQuery;
    }

    public void Run(PhotoLibrary library)
    {
        library.RemoveExcludedPhotos();
        this.UpdatePhotos(library);
    }

    private void UpdatePhotos(PhotoLibrary library)
    {
        var files = this.photosQuery.Run(library);
        var exceptions = new ConcurrentBag<Exception>();
        var newPhotos = new ConcurrentBag<Photo>();
        Parallel.ForEach(files, file =>
        {
            try
            {
                var relativePath = library.GetPathRelativeToSource(file.FullName);
                if (library.IsInExcludedFolder(relativePath))
                {
                    return;
                }

                var collectionPhoto = library.Collection.Photos.FirstOrDefault(x => x.RelativePath == relativePath);
                if (collectionPhoto is null)
                {
                    var newPhoto = Photo.Create(relativePath, file.Length);
                    newPhotos.Add(newPhoto);
                }
                else
                {
                    collectionPhoto.UpdateSizeBytes(file.Length);
                }
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        });

        if (exceptions.Any())
        {
            throw new AggregateException(exceptions.ToArray());
        }

        if (newPhotos.Any())
        {
            library.Collection.AddPhotos(newPhotos);
        }
    }
}
