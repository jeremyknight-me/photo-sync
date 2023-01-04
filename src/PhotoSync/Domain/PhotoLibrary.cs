using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

namespace PhotoSync.Domain;

internal sealed class PhotoLibrary
{
    private readonly List<string> excludedFolders = new();

    public string FileName { get; } = "photo-sync.db";

    [JsonInclude]
    public string DestinationFolder { get; private set; }

    [JsonIgnore]
    public string LibraryPath => Path.Combine(this.DestinationFolder, this.FileName);

    [JsonInclude]
    public string SourceFolder { get; private set; }

    [JsonInclude]
    public IEnumerable<string> ExcludedFolders
    {
        get => this.excludedFolders;
        private set
        {
            this.excludedFolders.Clear();
            this.excludedFolders.AddRange(value);
        }
    }

    public PhotoCollection Collection { get; private set; } = new();

    public static PhotoLibrary Create(string source, string destination)
        => new PhotoLibrary()
        {
            SourceFolder = source.Trim(),
            DestinationFolder = destination.Trim()
        };

    public void AddExcludedPath(string path)
    {
        var trimmed = path.Trim();
        if (this.excludedFolders.Contains(trimmed))
        {
            return;
        }

        this.excludedFolders.Add(trimmed);
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

    public bool IsInExcludedFolder(string relativePath)
    {
        foreach (var folder in this.ExcludedFolders)
        {
            if (relativePath.StartsWith(folder))
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveExcludedPath(string path)
    {
        var trimmed = path.Trim();
        if (this.excludedFolders.Contains(trimmed))
        {
            this.excludedFolders.Remove(trimmed);
        }
    }

    public void ToggleExcludedPath(string path)
    {
        if (this.excludedFolders.Contains(path))
        {
            this.RemoveExcludedPath(path);
        }
        else
        {
            this.AddExcludedPath(path);
        }
    }
}
