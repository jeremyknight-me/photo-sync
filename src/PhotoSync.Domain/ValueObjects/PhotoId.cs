namespace PhotoSync.Domain.ValueObjects;

public sealed record PhotoId : GuidIdBase
{
    private PhotoId(Guid value)
        : base(value)
    {
    }

    public static PhotoId New() => new(Guid.NewGuid());
    public static PhotoId New(Guid value) => new(value);
}
