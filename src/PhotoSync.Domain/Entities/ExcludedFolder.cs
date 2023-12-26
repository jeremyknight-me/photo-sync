using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Domain.Entities;

public sealed class ExcludedFolder : Entity<ExcludedFolderId>
{
    private ExcludedFolder() : base(ExcludedFolderId.New())
    {
    }

    public string RelativePath
    {
        get; init;
    }

    public static ExcludedFolder Create(string relativePath)
        => new()
        {
            RelativePath = relativePath.Trim()
        };
}
