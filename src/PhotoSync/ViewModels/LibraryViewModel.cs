using CommunityToolkit.Mvvm.ComponentModel;
using PhotoSync.Domain;

namespace PhotoSync.ViewModels;

[INotifyPropertyChanged]
public partial class LibraryViewModel
{
    public PhotoLibrary Library { get; private set; }

    public void SetLibrary(PhotoLibrary library) => this.Library = library;
}
