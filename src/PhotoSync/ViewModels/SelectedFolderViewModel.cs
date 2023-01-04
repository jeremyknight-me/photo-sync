namespace PhotoSync.ViewModels;

public sealed class SelectedFolderViewModel
{
    public string SourcePath { get; set; } = string.Empty;
    public string RelativePath { get; set; } = string.Empty;
    public bool IsExcluded { get; set; } = false;

    public void ToggleExcluded() => this.IsExcluded = !this.IsExcluded;
}
