using System.IO;
using System.Text.Json.Serialization;
using PhotoSync.Domain.Enums;

namespace PhotoSync.Domain;

internal sealed class Photo
{
    public string RelativePath { get; private set; } = null!;
    public PhotoAction ProcessAction { get; private set; } = PhotoAction.New;
    [JsonIgnore] public string Name => Path.GetFileName(this.RelativePath);
    public long SizeBytes { get; private set; }
}
