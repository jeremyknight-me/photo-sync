using System.Collections.Generic;
using System.IO;

namespace PhotoSync.Domain;

internal sealed class PhotoLibrary
{
    private readonly List<string> excludedFolders = new();

    public string FileName { get; private set; } = "photo-sync.db";
    public string DestinationFolder { get; private set; }
    public string LibraryPath => Path.Combine(this.DestinationFolder, this.FileName);
    public string SourceFolder { get; private set; }

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

    public void AddExcludedPath(string path)
    {
        var trimmed = path.Trim();
        if (this.excludedFolders.Contains(trimmed))
        {
            return;
        }

        this.excludedFolders.Add(trimmed);
    }

    public void RemoveExcludedPath(string path)
    {
        var trimmed = path.Trim();
        if (this.excludedFolders.Contains(trimmed))
        {
            this.excludedFolders.Remove(trimmed);
        }
    }
}
