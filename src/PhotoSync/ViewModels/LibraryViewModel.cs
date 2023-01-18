using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PhotoSync.Domain;
using PhotoSync.Domain.Contracts;
using PhotoSync.Domain.Extensions;

namespace PhotoSync.ViewModels;

[INotifyPropertyChanged]
public partial class LibraryViewModel
{
    private readonly IPhotoLibraryRepository libraryRepository;

    [ObservableProperty]
    private List<PhotoViewModel> currentPhotos = new();

    [ObservableProperty]
    private List<LibraryFolderViewModel> folders = new();

    [ObservableProperty]
    private string libraryFileName;

    [ObservableProperty]
    private string libraryDestinationFolder;
    
    public LibraryViewModel(IPhotoLibraryRepository photoLibraryRepository)
    {
        this.libraryRepository = photoLibraryRepository;
    }

    public PhotoLibrary Library { get; private set; }

    public void SetLibrary(PhotoLibrary library)
    {
        this.Library = library;
        this.LibraryDestinationFolder = library.DestinationFolder;
        this.LibraryFileName = library.FileName;
        this.LoadTreeFolders();
    }

    [RelayCommand]
    private void Save()
    {
        this.libraryRepository.Save(this.Library.FilePath, this.Library);
    } 

    [RelayCommand]
    private void SelectedItemChanged(object value)
    {
        if (value is null || value is not LibraryFolderViewModel)
        {
            return;
        }

        var vm = value as LibraryFolderViewModel;
        var photos = this.Library.Photos.Where(x => x.RelativeFolder == vm.RelativePath)
            .Select(x => new PhotoViewModel
            {
                Photo = x,
                FullPath = Path.Combine(this.Library.SourceFolder, x.RelativePath)
            })
            .ToList();
        this.CurrentPhotos = photos;
    }

    private void LoadTreeFolders()
    {
        var path = this.Library.SourceFolder;
        this.Folders.Clear();

        if (!Directory.Exists(path))
        {
            return;
        }

        var root = new DirectoryInfo(path);
        this.Folders = this.CreateFolderItems(root);
    }

    private List<LibraryFolderViewModel> CreateFolderItems(DirectoryInfo directoryInfo)
    {
        var folderItems = new List<LibraryFolderViewModel>();
        foreach (var directory in directoryInfo.GetDirectories())
        {
            var relativePath = this.Library.GetPathRelativeToSource(directory.FullName);
            var folder = new LibraryFolderViewModel
            {
                Name = directory.Name,
                FullPath = directory.FullName,
                RelativePath = relativePath,
                IsExcluded = this.Library.ExcludedFolders.Exists(relativePath),
                Children = this.CreateFolderItems(directory)
            };
            folderItems.Add(folder);
        }

        return folderItems;
    }
}
