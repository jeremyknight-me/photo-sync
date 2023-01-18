using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoSync.Domain.Operations;

public interface IGetPhotosOperation
{
    IReadOnlyList<FileInfo> Run(PhotoLibrary library);
}

public sealed class GetPhotosOperation : IGetPhotosOperation
{
    private readonly IEnumerable<string> extensions = new string[] { ".jpg", ".jpeg", ".png" };

    public IReadOnlyList<FileInfo> Run(PhotoLibrary library)
    {
        if (!Directory.Exists(library.SourceFolder))
        {
            return Enumerable.Empty<FileInfo>().ToList();
        }

        var directory = new DirectoryInfo(library.SourceFolder);
        var files = directory.GetFiles("*", SearchOption.AllDirectories);
        return files.AsParallel()
            .Where(x => this.extensions.Contains(x.Extension, StringComparer.OrdinalIgnoreCase))
            .ToList();
    }
}
