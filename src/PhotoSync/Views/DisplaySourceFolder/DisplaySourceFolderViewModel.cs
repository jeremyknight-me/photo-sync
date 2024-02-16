using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PhotoSync.Domain.Abstractions;
using PhotoSync.Domain.Entities;
using PhotoSync.Domain.Operations;
using PhotoSync.Domain.ValueObjects;
using PhotoSync.Views.DisplayLibrary;

namespace PhotoSync.Views.DisplaySourceFolder;

public partial class DisplaySourceFolderViewModel : ObservableObject
{
    private readonly IPhotoLibraryRepository libraryRepository;
    private readonly IRefreshSourceFolderOperation refreshOperation;

    [ObservableProperty]
    private List<PhotoViewModel> currentPhotos = [];

    [ObservableProperty]
    private List<LibraryFolderViewModel> folders = [];

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

    public DisplaySourceFolderViewModel(
        IPhotoLibraryRepository photoLibraryRepository,
        IRefreshSourceFolderOperation refreshLibraryOperation)
    {
        this.libraryRepository = photoLibraryRepository;
        this.refreshOperation = refreshLibraryOperation;
        this.excludedFolderCheckbox = new()
        {
            OnCheckedChanged = ToggleExcludeFolder
        };
    }

    public PhotoLibrary Library { get; private set; }
    public SourceFolder SourceFolder { get; private set; }

    public void SetLibrary(PhotoLibrary library, SourceFolderId sourceFolderId)
    {
        this.Library = library;
        this.SourceFolder = library.SourceFolders.FirstOrDefault(x => x.Id == sourceFolderId)
            ?? throw new ArgumentNullException("Source folder not found in library.");
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

    private bool CanIgnoreAll() => this.CurrentPhotos.Count != 0;

    [RelayCommand]
    private void Save()
    {
        this.refreshOperation.Run(this.SourceFolder);
        this.libraryRepository.Save(this.Library);
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
        foreach (var photo in this.CurrentPhotos)
        {
            photo.ProcessAction = Domain.Enums.PhotoAction.Sync;
        }
    }

    private bool CanSyncAll() => this.CurrentPhotos.Count != 0;

    private void ToggleExcludeFolder(bool isExcluded)
    {
        this.CurrentPhotos.Clear();
        if (isExcluded)
        {
            this.SourceFolder.AddExcludedFolder(this.CurrentFolder.RelativePath);
            this.PhotoTotalCount = 0;
            this.PhotoNewCount = 0;
            this.PhotoIgnoreCount = 0;
            this.PhotoSyncCount = 0;
        }
        else
        {
            this.SourceFolder.RemoveExcludedFolder(this.CurrentFolder.RelativePath);
            this.LoadFolderPhotos();
        }

        this.CurrentFolder.SetIsExcluded(isExcluded);
        this.OnPropertyChanged(nameof(this.CurrentPhotos));
    }

    private void LoadFolderPhotos()
    {
        var vm = this.CurrentFolder;
        var photos = this.SourceFolder.Photos.Where(x => x.RelativeFolder == vm.RelativePath)
            .Select(x => new PhotoViewModel
            {
                Photo = x,
                FullPath = Path.Combine(this.SourceFolder.FullPath, x.RelativePath)
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
        var path = this.SourceFolder.FullPath;
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
            var relativePath = this.SourceFolder.GetPathRelativeToSource(directory.FullName);
            var folder = new LibraryFolderViewModel
            {
                Name = directory.Name,
                FullPath = directory.FullName,
                RelativePath = relativePath,
                IsExcluded = this.SourceFolder.ExistsInExcludedFolders(relativePath),
                Parent = parent
            };
            folder.Children = this.CreateFolderItems(directory, folder);
            folderItems.Add(folder);
        }

        return folderItems;
    }
}
