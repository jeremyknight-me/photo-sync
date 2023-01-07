using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using PhotoSync.Domain.Extensions;

namespace PhotoSync.Domain.Operations;

public interface IRefreshLibraryOperation
{
    void Run(PhotoLibrary library);
}

public sealed class RefreshLibraryOperation : IRefreshLibraryOperation
{
    private readonly IGetPhotosOperation photosQuery;

    public RefreshLibraryOperation(IGetPhotosOperation getPhotosQuery)
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
                if (library.ExcludedFolders.Exists(relativePath))
                {
                    return;
                }

                var photo = library.Photos.FirstOrDefault(x => x.RelativePath == relativePath);
                if (photo is null)
                {
                    var newPhoto = Photo.Create(relativePath, file.Length);
                    newPhotos.Add(newPhoto);
                }
                else
                {
                    photo.UpdateSizeBytes(file.Length);
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
            library.AddPhotos(newPhotos);
        }

        library.UpdateLastRefreshed(DateTimeOffset.Now);
    }
}
