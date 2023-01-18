using CommunityToolkit.Mvvm.ComponentModel;
using PhotoSync.Domain;
using PhotoSync.Domain.Enums;

namespace PhotoSync.ViewModels;

[INotifyPropertyChanged]
public partial class PhotoViewModel
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
            this.OnPropertyChanged(nameof(ProcessAction));
        }
    }

    public Photo Photo { get; init; }
}
