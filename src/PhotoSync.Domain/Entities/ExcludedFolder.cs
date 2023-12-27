using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Domain.Entities;

public sealed class ExcludedFolder : Entity<ExcludedFolderId>
{
    private ExcludedFolder() : base(ExcludedFolderId.New())
    {
    }

    public required SourceFolderId SourceFolderId { get; init; }
    public SourceFolder SourceFolder { get; init; }
    public required string RelativePath { get; init; }

    public static ExcludedFolder Create(SourceFolderId sourceFolderId, string relativePath)
        => new()
        {
            SourceFolderId = sourceFolderId,
            RelativePath = relativePath.Trim()
        };
}
