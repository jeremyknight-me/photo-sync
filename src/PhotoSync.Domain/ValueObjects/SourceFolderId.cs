namespace PhotoSync.Domain.ValueObjects;

public sealed record SourceFolderId : GuidIdBase
{
    private SourceFolderId(Guid value)
        : base(value)
    {
    }

    public static SourceFolderId New() => new(Guid.NewGuid());
    public static SourceFolderId New(Guid value) => new(value);
}
