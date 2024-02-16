using System.Collections.Concurrent;
using PhotoSync.Domain.Entities;

namespace PhotoSync.Domain.Operations;

public interface IRefreshSourceFolderOperation
{
    void Run(SourceFolder sourceFolder);
}

public sealed class RefreshSourceFolderOperation : IRefreshSourceFolderOperation
{
    private readonly IGetPhotosOperation photosQuery;

    public RefreshSourceFolderOperation(IGetPhotosOperation getPhotosQuery)
    {
        this.photosQuery = getPhotosQuery;
    }

    public void Run(SourceFolder sourceFolder)
    {
        sourceFolder.CleanExcludedFolders();
        sourceFolder.CleanExcludedPhotos();
        this.UpdatePhotos(sourceFolder);
        sourceFolder.UpdateLastRefreshed(DateTimeOffset.Now);
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
                if (sourceFolder.ExistsInExcludedFolders(relativePath))
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

        if (!exceptions.IsEmpty)
        {
            throw new AggregateException(exceptions.ToArray());
        }

        if (!newPhotos.IsEmpty)
        {
            sourceFolder.AddPhotos(newPhotos);
        }
    }
}
