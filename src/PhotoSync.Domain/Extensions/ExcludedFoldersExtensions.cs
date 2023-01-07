using System.Collections.Generic;
using System.Linq;

namespace PhotoSync.Domain.Extensions;

public static class ExcludedFoldersExtensions
{
    public static bool Exists(this IReadOnlyCollection<ExcludedFolder> folders, string relativePath)
        => folders.Any(f => f.RelativePath.StartsWith(relativePath));
}
