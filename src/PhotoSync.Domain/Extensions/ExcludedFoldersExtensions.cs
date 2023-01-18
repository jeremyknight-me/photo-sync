using System.Collections.Generic;
using System.Linq;

namespace PhotoSync.Domain.Extensions;

public static class ExcludedFoldersExtensions
{
    public static bool Exists(this IEnumerable<ExcludedFolder> folders, string relativePath)
        => !string.IsNullOrWhiteSpace(relativePath)
            && folders.Any(f => relativePath.StartsWith(f.RelativePath));

    public static void Remove(this IList<ExcludedFolder> folders, IEnumerable<ExcludedFolder> foldersToRemove)
    {
        foreach (var folder in foldersToRemove)
        {
            if (folders.Any(x => x.Id == folder.Id))
            {
                folders.Remove(folder);
            }
        }
    }
}
