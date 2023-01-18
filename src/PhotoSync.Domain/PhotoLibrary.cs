using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using PhotoSync.Domain.Extensions;

namespace PhotoSync.Domain;

public sealed class PhotoLibrary : Entity<Guid>
{
    public const string DefaultLibraryFileName = "photo-sync.db";

    private readonly List<ExcludedFolder> excludedFolders = new();
    private readonly List<Photo> photos = new();

    private PhotoLibrary() : base(Guid.NewGuid())
    {
    }

    [JsonIgnore] public string FilePath { get; private set; }
    [JsonInclude] public DateTimeOffset? LastRefreshed { get; private set; } = null;
    [JsonInclude] public DateTimeOffset? LastSynced { get; private set; } = null;
    public string SourceFolder { get; init; }

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

    public string DestinationFolder => Path.GetDirectoryName(this.FilePath) ?? string.Empty;
    public string FileName => Path.GetFileName(this.FilePath);

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

        var folder = ExcludedFolder.Create(trimmed);
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

        this.excludedFolders.Remove(foldersToRemove);
    }

    public void CleanExcludedPhotos()
    {
        var roots = this.excludedFolders.Select(x => x.RelativePath).ToArray();
        this.photos.RemoveAll(p => roots.Any(r => p.RelativePath.StartsWith(r)));
    }

    public string GetPathRelativeToSource(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return string.Empty;
        }

        var sourcePathLength = this.SourceFolder.Length;
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
