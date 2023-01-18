using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PhotoSync.ViewModels;

[INotifyPropertyChanged]
public partial class CheckboxViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CheckedVisibility))]
    [NotifyPropertyChangedFor(nameof(UncheckedVisibility))]
    private bool isChecked = false;

    [ObservableProperty]
    private bool isEnabled = false;

    public Visibility CheckedVisibility => this.isChecked ? Visibility.Visible : Visibility.Collapsed;
    public Visibility UncheckedVisibility => this.isChecked ? Visibility.Collapsed : Visibility.Visible;

    public Action<bool> OnCheckedChanged { get; init; }

    partial void OnIsCheckedChanged(bool value)
    {
        this.OnCheckedChanged(value);
    }
}
