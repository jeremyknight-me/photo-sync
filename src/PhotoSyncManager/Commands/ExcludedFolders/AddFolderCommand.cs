using System;
using System.Linq;
using System.Windows.Forms;
using PhotoSync.Common.Entities;
using PhotoSyncManager.Models;
using PhotoSyncManager.ViewModels;

namespace PhotoSyncManager.Commands.ExcludedFolders
{
    internal class AddFolderCommand : RelayCommand
    {
        public AddFolderCommand()
            : base(ExecuteMethod, CanExecuteMethod)
        {
        }

        private static void ExecuteMethod(object parameter)
        {
            if (!(parameter is ExcludedFoldersViewModel))
            {
                throw new ArgumentException("Parameter must be the correct view model type.", nameof(parameter));
            }

            var dialog = new FolderBrowserDialog
            {
                Description = "Select folder to exclude",
                RootFolder = Environment.SpecialFolder.MyPictures,
                ShowNewFolderButton = false,
                UseDescriptionForTitle = true
            };
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var library = AppState.Instance.Library;
                var path = dialog.SelectedPath;
                var relativePath = path.Remove(0, library.SourceFolder.Length).TrimStart(new[] { '\\' });
                var viewModel = parameter as ExcludedFoldersViewModel;
                if (!viewModel.Folders.Any(x => x.RelativePath == relativePath))
                {
                    viewModel.Folders.Add(new ExcludeFolder { RelativePath = relativePath });
                }
            }
        }

        private static bool CanExecuteMethod(object parameter) => true;
    }
}
