using System;
using System.IO;
using PhotoSync.Domain.Enums;

namespace PhotoSync.Domain;

public sealed class Photo : Entity<Guid>
{
    private Photo()
        : base(Guid.NewGuid())
    {
    }

    public string RelativePath { get; private set; }
    public PhotoAction ProcessAction { get; private set; } = PhotoAction.New;
    public long SizeBytes { get; private set; }

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
