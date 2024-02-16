using CommunityToolkit.Mvvm.ComponentModel;
using PhotoSync.Domain.Entities;
using PhotoSync.Domain.Enums;

namespace PhotoSync.Views.DisplayLibrary;

public partial class PhotoViewModel : ObservableObject
{
    [ObservableProperty]
    private string fullPath;

    public PhotoAction ProcessAction
    {
        get => this.Photo.ProcessAction;
        set
        {
            if (this.Photo.ProcessAction.Equals(value))
            {
                return;
            }

            this.Photo.UpdateProcessAction(value);
            this.OnPropertyChanged(nameof(this.ProcessAction));
        }
    }

    public Photo Photo { get; init; }
}
