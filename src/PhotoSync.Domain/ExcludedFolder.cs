using System;

namespace PhotoSync.Domain;

public sealed class ExcludedFolder : Entity<Guid>
{
    private ExcludedFolder() : base(Guid.NewGuid())
    {
    }

    public string RelativePath { get; init; }

    public static ExcludedFolder Create(string relativePath)
        => new()
        {
            RelativePath = relativePath.Trim()
        };
}
