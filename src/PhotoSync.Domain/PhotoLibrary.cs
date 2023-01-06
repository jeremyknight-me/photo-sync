using System;
using System.Collections.Generic;
using System.IO;

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

    public static PhotoLibrary Create(string filePath, string sourceFolder)
        => new()
        {
            FilePath = filePath.Trim(),
            SourceFolder = sourceFolder.Trim()
        };
}
