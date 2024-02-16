using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Domain.Entities;

public sealed class SourceFolder : Entity<SourceFolderId>
{
    private readonly List<ExcludedFolder> excludedFolders = [];
    private readonly List<Photo> photos = [];

    private SourceFolder() : base(SourceFolderId.New())
    {
    }

    public required string FullPath { get; init; }
    public DateTimeOffset? LastRefreshed { get; private set; } = null;

    public IReadOnlyList<ExcludedFolder> ExcludedFolders
    {
        get => this.excludedFolders.AsReadOnly();
        init
        {
            this.excludedFolders.Clear();
            this.excludedFolders.AddRange(value);
        }
    }

    public IReadOnlyList<Photo> Photos
    {
        get => this.photos.AsReadOnly();
        init
        {
            this.photos.Clear();
            this.photos.AddRange(value);
        }
    }

    public static SourceFolder Create(string fullPath)
        => new() { FullPath = fullPath };

    public void AddPhotos(IEnumerable<Photo> newPhotos)
    {
        foreach (var photo in newPhotos)
        {
            if (this.photos.Any(p => p.RelativePath == photo.RelativePath))
            {
                continue;
            }

            this.photos.Add(photo);
        }
    }

    public void AddExcludedFolder(string relativePath)
    {
        var trimmed = relativePath.Trim();
        if (this.excludedFolders.Any(x => x.RelativePath == trimmed))
        {
            return;
        }

        var folder = ExcludedFolder.Create(this.Id, trimmed);
        this.excludedFolders.Add(folder);
    }

    public void CleanExcludedFolders()
    {
        var foldersToRemove = new List<ExcludedFolder>();
        foreach (var folder in this.excludedFolders)
        {
            if (this.excludedFolders.Any(x => folder.RelativePath.Length != x.RelativePath.Length
                && folder.RelativePath.StartsWith(x.RelativePath)))
            {
                foldersToRemove.Add(folder);
            }
        }

        this.RemoveExcludedFolders(foldersToRemove);
    }

    public void CleanExcludedPhotos()
    {
        var roots = this.excludedFolders.Select(x => x.RelativePath).ToArray();
        this.photos.RemoveAll(p => roots.Any(r => p.RelativePath.StartsWith(r)));
    }

    public bool ExistsInExcludedFolders(string relativePath)
        => !string.IsNullOrWhiteSpace(relativePath)
            && this.excludedFolders.Any(f => relativePath.StartsWith(f.RelativePath));

    public string GetPathRelativeToSource(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return string.Empty;
        }

        var sourcePathLength = this.FullPath.Length;
        return path.Remove(0, sourcePathLength).TrimStart(new[] { '\\' });
    }

    public void RemoveExcludedFolder(string relativePath)
    {
        var trimmed = relativePath.Trim();
        var folder = this.excludedFolders.SingleOrDefault(x => x.RelativePath == trimmed);
        if (folder is not null)
        {
            this.excludedFolders.Remove(folder);
        }
    }

    public void RemoveExcludedFolders(IEnumerable<ExcludedFolder> foldersToRemove)
    {
        foreach (var folder in foldersToRemove)
        {
            if (this.excludedFolders.Any(x => x.Id == folder.Id))
            {
                this.excludedFolders.Remove(folder);
            }
        }
    }

    public void UpdateLastRefreshed(DateTimeOffset date)
        => this.LastRefreshed = date;
}
