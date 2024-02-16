using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using PhotoSync.Domain.Entities;
using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.ViewModels;

[INotifyPropertyChanged]
public partial class SourceFolderViewModel
{
    private readonly PhotoLibrary library;
    private readonly SourceFolder folder;

    public SourceFolderViewModel(PhotoLibrary photoLibrary, SourceFolderId sourceFolderId)
    {
        this.library = photoLibrary;
        this.folder = photoLibrary.SourceFolders.FirstOrDefault(x => x.Id == sourceFolderId)
            ?? throw new ArgumentNullException(nameof(photoLibrary));
    }
}
