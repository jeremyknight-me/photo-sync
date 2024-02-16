using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Domain.Entities;

public sealed class PhotoLibrary : Entity<PhotoLibraryId>
{
    public const string DefaultLibraryFileName = "photo-sync.db";

    private readonly List<SourceFolder> sourceFolders = [];
    private readonly List<Destination> destinations = [];

    private PhotoLibrary() : base(PhotoLibraryId.New())
    {
    }

    public IReadOnlyList<Destination> Destinations
    {
        get => this.destinations.AsReadOnly();
        init
        {
            this.destinations.Clear();
            this.destinations.AddRange(value);
        }
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

    private string filePath = string.Empty;
    public string FilePath
    {
        get => this.filePath;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
            if (!string.IsNullOrWhiteSpace(this.filePath))
            {
                throw new InvalidOperationException("File path can only be set on a newly opened library.");
            }

            var trimmed = value.Trim();
            if (!File.Exists(trimmed))
            {
                throw new FileNotFoundException("File does not exist.");
            }

            this.filePath = trimmed;
        }
    }

    public DateTimeOffset? LastRefreshed => this.sourceFolders.Count > 0 ? this.sourceFolders.Max(x => x.LastRefreshed) : null;
    
    public string FileName => Path.GetFileName(this.FilePath);

    public static PhotoLibrary Create(string filePath)
        => new() { FilePath = filePath.Trim() };
}
