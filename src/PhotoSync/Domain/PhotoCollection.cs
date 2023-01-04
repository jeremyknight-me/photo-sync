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

    public void RemoveWithRoots(IEnumerable<string> roots)
    {
        Stack<int> indexes = new();
        for (var i = 0; i < this.photos.Count; i++)
        {
            var photo = this.photos[i];
            if (roots.Any(x => photo.RelativePath.StartsWith(x)))
            {
                indexes.Push(i);
            }
        }

        while (indexes.Count > 0)
        {
            var index = indexes.Pop();
            this.photos.RemoveAt(index);
        }
    }
}
