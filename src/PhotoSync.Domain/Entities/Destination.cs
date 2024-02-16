using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Domain.Entities;

public sealed class Destination : Entity<DestinationId>
{
    public required string FullPath { get; init; }
    public DateTimeOffset? LastSynced { get; set; }
}
