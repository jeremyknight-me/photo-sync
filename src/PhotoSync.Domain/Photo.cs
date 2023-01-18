using System;
using System.IO;
using System.Text.Json.Serialization;
using PhotoSync.Domain.Enums;

namespace PhotoSync.Domain;

public sealed class Photo : Entity<Guid>
{
    private Photo() : base(Guid.NewGuid())
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
