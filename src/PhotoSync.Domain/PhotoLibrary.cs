using System;
using System.IO;

namespace PhotoSync.Domain;

public sealed class PhotoLibrary : Entity<Guid>
{
    public const string DefaultLibraryFileName = "photo-sync.db";

    private PhotoLibrary()
        : base(Guid.NewGuid())
    {
    }

    public string FilePath { get; private set; }
    public DateTimeOffset? LastRefreshed { get; private set; }
    public DateTimeOffset? LastSynced { get; private set; } = null;
    public string SourceFolder { get; private set; }

    public string DestinationFolder => Path.GetDirectoryName(this.DestinationFolder) ?? string.Empty;
    public string FileName => Path.GetFileName(this.FilePath);

    public static PhotoLibrary Create(string filePath, string sourceFolder)
        => new()
        {
            FilePath = filePath.Trim(),
            SourceFolder = sourceFolder.Trim()
        };
}
