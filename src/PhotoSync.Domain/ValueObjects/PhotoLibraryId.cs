namespace PhotoSync.Domain.ValueObjects;

public sealed record PhotoLibraryId : GuidIdBase
{
    private PhotoLibraryId(Guid value)
        : base(value)
    {
    }

    public static PhotoLibraryId New() => new(Guid.NewGuid());
    public static PhotoLibraryId New(Guid value) => new(value);
}
