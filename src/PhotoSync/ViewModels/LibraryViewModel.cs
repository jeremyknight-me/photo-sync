using System.Collections.Generic;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using PhotoSync.Domain;
using PhotoSync.Domain.Extensions;
using PhotoSync.Models;

namespace PhotoSync.ViewModels;

[INotifyPropertyChanged]
public partial class LibraryViewModel
{
    [ObservableProperty]
    private List<LibraryFolder> folders = new();

    [ObservableProperty]
    private string libraryFileName;

    [ObservableProperty]
    private string libraryDestinationFolder;

    public PhotoLibrary Library { get; private set; }

    public void SetLibrary(PhotoLibrary library)
    {
        this.Library = library;
        this.LibraryDestinationFolder = library.DestinationFolder;
        this.LibraryFileName = library.FileName;
        this.LoadTreeFolders();
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

    private List<LibraryFolder> CreateFolderItems(DirectoryInfo directoryInfo)
    {
        var folderItems = new List<LibraryFolder>();
        foreach (var directory in directoryInfo.GetDirectories())
        {
            var relativePath = this.Library.GetPathRelativeToSource(directoryInfo.FullName);
            var folder = new LibraryFolder
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
