namespace PhotoSync.Domain.ValueObjects;

public sealed record DestinationId : GuidIdBase
{
    private DestinationId(Guid value)
        : base(value)
    {
    }

    public static DestinationId New() => new(Guid.NewGuid());
    public static DestinationId New(Guid value) => new(value);
}
