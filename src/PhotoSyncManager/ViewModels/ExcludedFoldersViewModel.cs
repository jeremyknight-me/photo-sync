using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PhotoSync.Common;
using PhotoSync.Common.Entities;
using PhotoSyncManager.Commands;
using PhotoSyncManager.Commands.ExcludedFolders;
using PhotoSyncManager.Models;

namespace PhotoSyncManager.ViewModels
{
    public class ExcludedFoldersViewModel
    {
        public ExcludedFoldersViewModel(MainViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;

            var library = AppState.Instance.Library;
            if (library is null)
            {
                this.Folders = new ObservableCollection<ExcludeFolder>();
            }
            else
            {
                using var context = PhotoSyncContextFactory.Make(library.DestinationFullPath);
                this.Folders = new ObservableCollection<ExcludeFolder>(context.ExcludeFolders.ToArray());
            }
        }

        public ICommand CloseCommand { get; private set; } = new CloseWindowCommand();
        public ICommand SaveCommand { get; private set; } = new SaveFoldersCommand();
        public ICommand AddCommand { get; private set; } = new AddFolderCommand();
        public ICommand RemoveCommand { get; private set; } = new RemoveFolderCommand();

        public ObservableCollection<ExcludeFolder> Folders { get; private set; }

        public MainViewModel MainViewModel { get; private set; }
        public ExcludeFolder SelectedFolder { get; set; }
    }
}
