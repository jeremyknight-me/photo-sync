using System.Collections.Generic;

namespace PhotoSync.ViewModels;

public partial class LibraryFolderViewModel
{
    public string Name { get; set; }
    public string FullPath { get; set; }
    public string RelativePath { get; set; }
    public bool IsExcluded { get; set; }
    public List<LibraryFolderViewModel> Children { get; set; } = new();
}
