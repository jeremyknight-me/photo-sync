using System.Text.Json.Serialization;
using PhotoSync.Domain.Enums;
using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Domain.Entities;

public sealed class Photo : Entity<PhotoId>
{
    private Photo() : base(PhotoId.New())
    {
    }

    public string RelativePath { get; init; }

    [JsonInclude] public PhotoAction ProcessAction { get; private set; } = PhotoAction.New;
    [JsonInclude] public long SizeBytes { get; private set; }

    public string Name => Path.GetFileName(this.RelativePath);
    public string RelativeFolder => Path.GetDirectoryName(this.RelativePath);

    public void UpdateProcessAction(PhotoAction action) => this.ProcessAction = action;
    public void UpdateSizeBytes(long size) => this.SizeBytes = size;

    public static Photo Create(string relativePath, long sizeBytes)
        => new()
        {
            RelativePath = relativePath.Trim(),
            SizeBytes = sizeBytes
        };
}
