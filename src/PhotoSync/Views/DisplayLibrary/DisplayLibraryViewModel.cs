using CommunityToolkit.Mvvm.ComponentModel;
using PhotoSync.Domain.Entities;

namespace PhotoSync.Views.DisplayLibrary;

public partial class DisplayLibraryViewModel : ObservableObject
{
    public PhotoLibrary Library { get; private set; }

    public void SetLibrary(PhotoLibrary library)
    {
        this.Library = library;
    }
}
