using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Domain.Entities;

public sealed class PhotoLibrary : Entity<PhotoLibraryId>
{
    public const string DefaultLibraryFileName = "photo-sync.db";

    private readonly List<SourceFolder> sourceFolders = [];

    private PhotoLibrary() : base(PhotoLibraryId.New())
    {
    }

    public IReadOnlyList<SourceFolder> SourceFolders
    {
        get => this.sourceFolders.AsReadOnly();
        init
        {
            this.sourceFolders.Clear();
            this.sourceFolders.AddRange(value);
        }
    }

    public string FilePath { get; private set; }
    public DateTimeOffset? LastRefreshed { get; private set; } = null;
    public DateTimeOffset? LastSynced { get; private set; } = null;

    public string DestinationFolder => Path.GetDirectoryName(this.FilePath) ?? string.Empty;
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

    public void UpdateLastRefreshed(DateTimeOffset date)
        => this.LastRefreshed = date;

    public static PhotoLibrary Create(string filePath)
        => new() { FilePath = filePath.Trim() };
}
