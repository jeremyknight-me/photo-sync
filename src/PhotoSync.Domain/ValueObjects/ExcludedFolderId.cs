namespace PhotoSync.Domain.ValueObjects;

public sealed record ExcludedFolderId : GuidIdBase
{
    private ExcludedFolderId(Guid value)
        : base(value)
    {
    }

    public static ExcludedFolderId New() => new(Guid.NewGuid());
    public static ExcludedFolderId New(Guid value) => new(value);
}
