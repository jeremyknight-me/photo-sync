using System;
using System.Windows;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PhotoSync.Domain;
using PhotoSync.Views;

namespace PhotoSync.ViewModels;

[INotifyPropertyChanged]
public partial class CreateLibraryViewModel
{
    private const string destinationFileTooltip = "Destination file name";
    private const string destinationFolderTooltip = "Destination folder path";
    private readonly IServiceProvider services;

    [ObservableProperty]
    private string destinationFolder;

    [ObservableProperty]
    private string destinationFileName = PhotoLibrary.DefaultLibraryFileName;

    [ObservableProperty]
    private string sourceFolder;

    public CreateLibraryViewModel(IServiceProvider serviceProvider)
    {
        this.services = serviceProvider;
    }

    public string DestinationFileTooltip => destinationFileTooltip;
    public string DestinationFolderTooltip => destinationFolderTooltip;

    [RelayCommand]
    private void Cancel(Window currentWindow)
    {
        var next = this.services.GetRequiredService<MainWindow>();
        next.Show();
        currentWindow.Close();
    }

    [RelayCommand]
    private void Create(Window currentWindow)
    {
        // todo: create and refresh library

        var next = this.services.GetRequiredService<LibraryWindow>();
        next.Show();
        currentWindow.Close();
    }

    [RelayCommand]
    private void GetDestinationFolderPath()
    {
        var path = this.GetFolderPath("Select destination folder");
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        this.DestinationFolder = path;
    }

    [RelayCommand]
    private void GetSourceFolderPath()
    {
        var path = this.GetFolderPath("Select source folder");
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        this.SourceFolder = path;
    }

    private string GetFolderPath(string description)
    {
        var dialog = new FolderBrowserDialog
        {
            Description = description,
            RootFolder = Environment.SpecialFolder.MyPictures,
            ShowNewFolderButton = false,
            UseDescriptionForTitle = true
        };
        var result = dialog.ShowDialog();
        if (result == DialogResult.OK)
        {
            var path = dialog.SelectedPath;
            if (!string.IsNullOrEmpty(path))
            {
                return path;
            }
        }

        return string.Empty;
    }
}
