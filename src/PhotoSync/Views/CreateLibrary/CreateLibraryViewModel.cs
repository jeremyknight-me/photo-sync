using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using PhotoSync.Domain.Abstractions;
using PhotoSync.Domain.Entities;
using PhotoSync.Extensions;

namespace PhotoSync.Views.CreateLibrary;

public partial class CreateLibraryViewModel : ObservableObject
{
    private const string destinationFileTooltip = "Destination file name";
    private const string destinationFolderTooltip = "Destination folder path";

    private readonly IServiceProvider services;
    private readonly IPhotoLibraryRepository libraryRepository;

    [ObservableProperty]
    private string destinationFolder;

    [ObservableProperty]
    private string destinationFileName = PhotoLibrary.DefaultLibraryFileName;

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
        var next = this.services.GetRequiredService<Main.MainWindow>();
        next.Show();
        currentWindow.Close();
    }

    [RelayCommand]
    private void Create(Window currentWindow)
    {
        var errors = this.Validate();
        if (errors.Any())
        {
            var validationWindow = new DisplayValidationError.DisplayValidationErrorWindow(errors);
            validationWindow.ShowDialog();
            return;
        }

        var filePath = Path.Combine(this.DestinationFolder, this.DestinationFileName);
        var library = this.libraryRepository.Create(filePath);

        var next = this.services.GetRequiredService<Views.DisplayLibrary.DisplayLibraryWindow>();
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

    private string GetFolderPath(string description)
    {
        var myPictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        var dialog = new OpenFolderDialog
        {
            Multiselect = false,
            InitialDirectory = myPictures,
            Title = description,
            DefaultDirectory = myPictures,
            AddToRecent = false
        };
        var result = dialog.ShowDialog();
        if (result == true)
        {
            var path = dialog.FolderName;
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

        if (string.IsNullOrWhiteSpace(this.DestinationFileName))
        {
            errors.AddError("Destination", "Destination requires a file name");
        }

        if (string.IsNullOrWhiteSpace(this.DestinationFolder))
        {
            errors.AddError("Destination", "Destination requires a folder path");
        }

        return errors;
    }
}
