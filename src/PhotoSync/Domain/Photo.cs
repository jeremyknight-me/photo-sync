using System.IO;
using System.Text.Json.Serialization;

namespace PhotoSync.Domain;

internal sealed class Photo
{
    public required string RelativePath { get; init; }
    [JsonIgnore] public string RelativeFolder => Path.GetDirectoryName(this.RelativePath);
    public PhotoAction ProcessAction { get; private set; } = PhotoAction.New;
    [JsonIgnore] public string Name => Path.GetFileName(this.RelativePath);
    [JsonInclude] public long SizeBytes { get; private set; }

    [JsonIgnore]
    public int BindAction
    {
        get => (int)this.ProcessAction;
        set => this.ProcessAction = (PhotoAction)value;
    }

    public void UpdateAction(PhotoAction action) => this.ProcessAction = action;
    public void UpdateSizeBytes(long size) => this.SizeBytes = size;

    public static Photo Create(string relativePath, long sizeBytes)
        => new Photo()
        {
            RelativePath = relativePath,
            SizeBytes = sizeBytes
        };
}
