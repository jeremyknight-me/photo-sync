using PhotoSync.Domain.Entities;

namespace PhotoSync.Domain.Operations;

public interface IGetPhotosOperation
{
    IReadOnlyList<FileInfo> Run(SourceFolder sourceFolder);
}

public sealed class GetPhotosOperation : IGetPhotosOperation
{
    private readonly IEnumerable<string> extensions = new string[] { ".jpg", ".jpeg", ".png" };

    public IReadOnlyList<FileInfo> Run(SourceFolder sourceFolder)
    {
        if (!Directory.Exists(sourceFolder.FullPath))
        {
            return Enumerable.Empty<FileInfo>().ToList();
        }

        var directory = new DirectoryInfo(sourceFolder.FullPath);
        var files = directory.GetFiles("*", SearchOption.AllDirectories);
        return files.AsParallel()
            .Where(x => this.extensions.Contains(x.Extension, StringComparer.OrdinalIgnoreCase))
            .ToList();
    }
}
