using System;
using System.Windows;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PhotoSync.Domain.Abstractions;
using PhotoSync.Views;

namespace PhotoSync.ViewModels;

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
        var next = this.services.GetRequiredService<LibraryWindow>();
        next.ViewModel.SetLibrary(library);
        next.Show();
        currentWindow.Close();
    }

    [RelayCommand]
    private void CreateLibrary(Window currentWindow)
    {
        var next = this.services.GetRequiredService<CreateLibraryWindow>();
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
            Title = "Open Library"
        };

        var result = dialog.ShowDialog();
        if (result == DialogResult.OK)
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
