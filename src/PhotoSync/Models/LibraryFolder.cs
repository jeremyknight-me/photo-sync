using System.Collections.Generic;

namespace PhotoSync.Models;

public sealed class LibraryFolder
{
    public string Name { get; set; }
    public string FullPath { get; set; }
    public string RelativePath { get; set; }
    public bool IsExcluded { get; set; }
    public List<LibraryFolder> Children { get; set; } = new();
}
