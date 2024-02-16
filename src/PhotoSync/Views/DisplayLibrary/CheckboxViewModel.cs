using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PhotoSync.Views.DisplayLibrary;

public partial class CheckboxViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CheckedVisibility))]
    [NotifyPropertyChangedFor(nameof(UncheckedVisibility))]
    private bool isChecked = false;

    [ObservableProperty]
    private bool isEnabled = false;

    public Visibility CheckedVisibility => this.IsChecked ? Visibility.Visible : Visibility.Collapsed;
    public Visibility UncheckedVisibility => this.IsChecked ? Visibility.Collapsed : Visibility.Visible;

    public Action<bool> OnCheckedChanged { get; init; }

    partial void OnIsCheckedChanged(bool value)
    {
        this.OnCheckedChanged(value);
    }
}
