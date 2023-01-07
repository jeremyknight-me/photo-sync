using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PhotoSync.Domain.Extensions;

namespace PhotoSync.Domain;

public sealed class PhotoLibrary : Entity<Guid>
{
    public const string DefaultLibraryFileName = "photo-sync.db";

    private readonly List<ExcludedFolder> excludedFolders = new();
    private readonly List<Photo> photos = new();

    private PhotoLibrary()
        : base(Guid.NewGuid())
    {
    }

    public string FilePath { get; private set; }
    public DateTimeOffset? LastRefreshed { get; private set; }
    public DateTimeOffset? LastSynced { get; private set; } = null;
    public string SourceFolder { get; private set; }

    public IReadOnlyCollection<ExcludedFolder> ExcludedFolders => this.excludedFolders.AsReadOnly();
    public IReadOnlyCollection<Photo> Photos => this.photos.AsReadOnly();

    public string DestinationFolder => Path.GetDirectoryName(this.DestinationFolder) ?? string.Empty;
    public string FileName => Path.GetFileName(this.FilePath);

    public void AddPhotos(IEnumerable<Photo> photos) => this.photos.AddPhotos(photos);

    public string GetPathRelativeToSource(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return string.Empty;
        }

        var sourcePathLength = this.SourceFolder.Length;
        return path.Remove(0, sourcePathLength).TrimStart(new[] { '\\' });
    }

    public void RemoveExcludedPhotos()
    {
        var roots = this.excludedFolders.Select(x => x.RelativePath).ToArray();
        this.photos.RemoveAll(p => roots.Any(r => p.RelativePath.StartsWith(r)));
    }

    public void SetFilePath(string fileName)
    {
        ArgumentException.ThrowIfNullOrEmpty(fileName, nameof(fileName));
        if (!string.IsNullOrWhiteSpace(this.FilePath))
        {
            throw new InvalidOperationException("File path can only be set on a newly opened library.");
        }

        var trimmed = fileName.Trim();
        if (!File.Exists(trimmed))
        {
            throw new FileNotFoundException("File does not exist.");
        }

        this.FilePath = trimmed;
    }

    public void UpdateLastRefreshed(DateTimeOffset date) => this.LastRefreshed = date;

    public static PhotoLibrary Create(string filePath, string sourceFolder)
        => new()
        {
            FilePath = filePath.Trim(),
            SourceFolder = sourceFolder.Trim()
        };
}
