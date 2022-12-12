using System.Collections.Generic;
using System.IO;

namespace PhotoSync.Common;

public class PhotoLibrary
{
    public string DestinationFullPath => Path.Combine(this.Settings.DestinationFolder, this.FileName);
    public string FileName { get; set; } = "photo-sync.db";
    public Settings Settings { get; set; } = new();
    public List<string> ExcludeFolders { get; set; } = new();
    public List<Photo> Photos { get; set; }
}
