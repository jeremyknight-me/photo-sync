using System.Collections.Generic;

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
}
