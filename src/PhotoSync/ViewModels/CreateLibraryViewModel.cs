using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PhotoSync.Domain.Abstractions;
using PhotoSync.Domain.Entities;
using PhotoSync.Extensions;
using PhotoSync.Views;

namespace PhotoSync.ViewModels;

[INotifyPropertyChanged]
public partial class CreateLibraryViewModel
{
    private const string destinationFileTooltip = "Destination file name";
    private const string destinationFolderTooltip = "Destination folder path";

    private readonly IServiceProvider services;
    private readonly IPhotoLibraryRepository libraryRepository;

    [ObservableProperty]
    private string destinationFolder;

    [ObservableProperty]
    private string destinationFileName = PhotoLibrary.DefaultLibraryFileName;

    [ObservableProperty]
    private string sourceFolder;

    public CreateLibraryViewModel(
        IServiceProvider serviceProvider,
        IPhotoLibraryRepository photoLibraryRepository)
    {
        this.services = serviceProvider;
        this.libraryRepository = photoLibraryRepository;
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
        var errors = this.Validate();
        if (errors.Any())
        {
            var validationWindow = new ValidationErrorWindow(errors);
            validationWindow.ShowDialog();
            return;
        }

        var filePath = Path.Combine(this.destinationFolder, this.destinationFileName);
        var library = this.libraryRepository.Create(filePath, this.sourceFolder);

        var next = this.services.GetRequiredService<LibraryWindow>();
        next.ViewModel.SetLibrary(library);
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

    private IDictionary<string, IList<string>> Validate()
    {
        var errors = new Dictionary<string, IList<string>>();

        if (string.IsNullOrWhiteSpace(this.destinationFileName))
        {
            errors.AddError("Destination", "Destination requires a file name");
        }

        if (string.IsNullOrWhiteSpace(this.destinationFolder))
        {
            errors.AddError("Destination", "Destination requires a folder path");
        }

        if (string.IsNullOrWhiteSpace(this.sourceFolder))
        {
            errors.AddError("Source", "Source requires a folder path");
        }

        return errors;
    }
}
