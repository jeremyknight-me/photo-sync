using System.Collections.Generic;
using System.Linq;

namespace PhotoSync.Domain;

internal sealed class PhotoCollection
{
    private readonly List<Photo> photos = new();

    public IEnumerable<Photo> Photos
    {
        get => this.photos;
        set
        {
            this.photos.Clear();
            this.photos.AddRange(value);
        }
    }

    public void AddPhotos(IEnumerable<Photo> photos)
    {
        foreach (var photo in photos)
        {
            if (this.photos.Any(x => x.RelativePath == photo.RelativePath))
            {
                continue;
            }

            this.photos.Add(photo);
        }
    }
}
