using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using PhotoSync.Domain.Abstractions;

namespace PhotoSync.Views.Main;

public partial class MainViewModel
{
    private readonly IServiceProvider services;
    private readonly IPhotoLibraryRepository libraryRepository;

    public MainViewModel(
        IServiceProvider serviceProvider,
        IPhotoLibraryRepository photoLibraryRepository)
    {
        this.services = serviceProvider;
        this.libraryRepository = photoLibraryRepository;
    }

    [RelayCommand]
    private void OpenLibrary(Window currentWindow)
    {
        var path = this.GetFilePath();
        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        var library = this.libraryRepository.Open(path);
        var next = this.services.GetRequiredService<DisplayLibrary.DisplayLibraryWindow>();
        next.ViewModel.SetLibrary(library);
        next.Show();
        currentWindow.Close();
    }

    [RelayCommand]
    private void CreateLibrary(Window currentWindow)
    {
        var next = this.services.GetRequiredService<CreateLibrary.CreateLibraryWindow>();
        next.Show();
        currentWindow.Close();
    }

    private string GetFilePath()
    {
        var dialog = new OpenFileDialog
        {
            DefaultExt = "db",
            CheckFileExists = true,
            CheckPathExists = true,
            Filter = "db files (*.db)|*.db|All Files (*.*)|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            Multiselect = false,
            Title = "Open Library",
            AddToRecent = false
        };

        var result = dialog.ShowDialog();
        if (result == true)
        {
            var path = dialog.FileName;
            if (!string.IsNullOrWhiteSpace(path))
            {
                return path;
            }
        }

        return string.Empty;
    }
}
