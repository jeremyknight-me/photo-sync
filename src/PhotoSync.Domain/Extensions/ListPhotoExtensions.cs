using System.Collections.Generic;
using System.Linq;

namespace PhotoSync.Domain.Extensions;

internal static class ListPhotoExtensions
{
    internal static void AddPhotos(this List<Photo> photoList, IEnumerable<Photo> newPhotos)
    {
        foreach (var photo in newPhotos)
        {
            if (photoList.Any(p => p.RelativePath == photo.RelativePath))
            {
                continue;
            }

            photoList.Add(photo);
        }
    }
}
