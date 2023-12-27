using System.Collections.Concurrent;
using PhotoSync.Domain.Entities;
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
        foreach (var sourceFolder in library.SourceFolders)
        {
            sourceFolder.CleanExcludedFolders();
            sourceFolder.CleanExcludedPhotos();
            this.UpdatePhotos(sourceFolder);
        }

        library.UpdateLastRefreshed(DateTimeOffset.Now);
    }

    private void UpdatePhotos(SourceFolder sourceFolder)
    {
        var files = this.photosQuery.Run(sourceFolder);
        var exceptions = new ConcurrentBag<Exception>();
        var newPhotos = new ConcurrentBag<Photo>();
        Parallel.ForEach(files, file =>
        {
            try
            {
                var relativePath = sourceFolder.GetPathRelativeToSource(file.FullName);
                if (sourceFolder.ExcludedFolders.Exists(relativePath))
                {
                    return;
                }

                var photo = sourceFolder.Photos.FirstOrDefault(x => x.RelativePath == relativePath);
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
            sourceFolder.AddPhotos(newPhotos);
        }
    }
}
