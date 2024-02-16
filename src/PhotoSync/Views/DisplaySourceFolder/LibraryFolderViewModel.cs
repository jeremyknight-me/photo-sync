namespace PhotoSync.Views.DisplaySourceFolder;

public partial class LibraryFolderViewModel
{
    private bool isExcluded;

    public string Name { get; init; }
    public string FullPath { get; init; }
    public string RelativePath { get; init; }
    public List<LibraryFolderViewModel> Children { get; set; } = new();
    public LibraryFolderViewModel Parent { get; init; } = null;

    public bool IsExcluded
    {
        get => this.Parent is not null && this.Parent.IsExcluded || this.isExcluded;
        init => this.isExcluded = value;
    }

    public void SetIsExcluded(bool value) => this.isExcluded = value;
}
