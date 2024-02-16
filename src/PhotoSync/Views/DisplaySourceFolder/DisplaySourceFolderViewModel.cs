using CommunityToolkit.Mvvm.ComponentModel;
using PhotoSync.Domain.Entities;
using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.ViewModels;

public partial class DisplaySourceFolderViewModel : ObservableObject
{
    private readonly PhotoLibrary library;
    private readonly SourceFolder folder;

    public DisplaySourceFolderViewModel(PhotoLibrary photoLibrary, SourceFolderId sourceFolderId)
    {
        this.library = photoLibrary;
        this.folder = photoLibrary.SourceFolders.FirstOrDefault(x => x.Id == sourceFolderId)
            ?? throw new ArgumentNullException(nameof(photoLibrary));
    }
}
