using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PhotoSync.Domain;
using PhotoSync.Domain.Contracts;
using PhotoSync.Domain.Extensions;
using PhotoSync.Domain.Operations;

namespace PhotoSync.ViewModels;

[INotifyPropertyChanged]
public partial class LibraryViewModel
{
    private readonly IPhotoLibraryRepository libraryRepository;
    private readonly IRefreshLibraryOperation refreshOperation;

    [ObservableProperty]
    private List<PhotoViewModel> currentPhotos = new();

    [ObservableProperty]
    private List<LibraryFolderViewModel> folders = new();

    [ObservableProperty]
    private string libraryFileName;

    [ObservableProperty]
    private string libraryDestinationFolder;

    [ObservableProperty]
    private LibraryFolderViewModel currentFolder = null;

    [ObservableProperty]
    private CheckboxViewModel excludedFolderCheckbox;

    [ObservableProperty]
    private int photoTotalCount = 0;

    [ObservableProperty]
    private int photoNewCount = 0;

    [ObservableProperty]
    private int photoIgnoreCount = 0;

    [ObservableProperty]
    private int photoSyncCount = 0;

    public LibraryViewModel(
        IPhotoLibraryRepository photoLibraryRepository,
        IRefreshLibraryOperation refreshLibraryOperation)
    {
        this.libraryRepository = photoLibraryRepository;
        this.refreshOperation = refreshLibraryOperation;
        this.excludedFolderCheckbox = new()
        {
            OnCheckedChanged = ToggleExcludeFolder
        };
    }

    public PhotoLibrary Library { get; private set; }

    public void SetLibrary(PhotoLibrary library)
    {
        this.Library = library;
        this.LibraryDestinationFolder = library.DestinationFolder;
        this.LibraryFileName = library.FileName;
        this.LoadTreeFolders();
    }

    [RelayCommand(CanExecute = nameof(CanIgnoreAll))]
    private void IgnoreAll()
    {
        foreach (var photo in this.CurrentPhotos)
        {
            photo.ProcessAction = Domain.Enums.PhotoAction.Ignore;
        }
    }

    private bool CanIgnoreAll() => this.CurrentPhotos.Any();

    [RelayCommand]
    private void Save()
    {
        this.refreshOperation.Run(this.Library);
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
        this.CurrentFolder = vm;
        this.ExcludedFolderCheckbox.IsEnabled = this.CurrentFolder is not null;
        this.ExcludedFolderCheckbox.IsChecked = vm.IsExcluded;
        this.IgnoreAllCommand.NotifyCanExecuteChanged();
        this.SyncAllCommand.NotifyCanExecuteChanged();
        if (!vm.IsExcluded)
        {
            this.LoadFolderPhotos();
        }
    }

    [RelayCommand(CanExecute = nameof(CanSyncAll))]
    private void SyncAll()
    {
        foreach(var photo in this.CurrentPhotos)
        {
            photo.ProcessAction = Domain.Enums.PhotoAction.Sync;
        }
    }

    private bool CanSyncAll() => this.CurrentPhotos.Any();

    private void ToggleExcludeFolder(bool isExcluded)
    {
        this.CurrentPhotos.Clear();
        if (isExcluded)
        {
            this.Library.AddExcludedFolder(this.currentFolder.RelativePath);
            this.PhotoTotalCount = 0;
            this.PhotoNewCount = 0;
            this.PhotoIgnoreCount = 0;
            this.PhotoSyncCount = 0;
        }
        else
        {
            this.Library.RemoveExcludedFolder(this.currentFolder.RelativePath);
            this.LoadFolderPhotos();
        }

        this.CurrentFolder.SetIsExcluded(isExcluded);
        this.OnPropertyChanged(nameof(this.CurrentPhotos));
    }

    private void LoadFolderPhotos()
    {
        var vm = this.CurrentFolder;
        var photos = this.Library.Photos.Where(x => x.RelativeFolder == vm.RelativePath)
            .Select(x => new PhotoViewModel
            {
                Photo = x,
                FullPath = Path.Combine(this.Library.SourceFolder, x.RelativePath)
            })
            .ToList();
        this.CurrentPhotos = photos;
        this.PhotoTotalCount = photos.Count();
        this.PhotoNewCount = photos.Count(x => x.ProcessAction == Domain.Enums.PhotoAction.New);
        this.PhotoIgnoreCount = photos.Count(x => x.ProcessAction == Domain.Enums.PhotoAction.Ignore);
        this.PhotoSyncCount = photos.Count(x => x.ProcessAction == Domain.Enums.PhotoAction.Sync);
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

    private List<LibraryFolderViewModel> CreateFolderItems(DirectoryInfo directoryInfo, LibraryFolderViewModel parent = null)
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
                Parent = parent
            };
            folder.Children = this.CreateFolderItems(directory, folder);
            folderItems.Add(folder);
        }

        return folderItems;
    }
}
