using System;
using PhotoSyncManager.ViewModels;

namespace PhotoSyncManager.Commands.ExcludedFolders
{
    internal class RemoveFolderCommand : RelayCommand
    {
        public RemoveFolderCommand()
            : base(ExecuteMethod, CanExecuteMethod)
        {
        }

        private static void ExecuteMethod(object parameter)
        {
            if (!(parameter is ExcludedFoldersViewModel))
            {
                throw new ArgumentException("Parameter must be the correct view model type.", nameof(parameter));
            }

            var viewModel = parameter as ExcludedFoldersViewModel;
            viewModel.Folders.Remove(viewModel.SelectedFolder);
        }

        private static bool CanExecuteMethod(object parameter)
            => !(parameter is ExcludedFoldersViewModel)
                ? throw new ArgumentException("Parameter must be the correct view model type.", nameof(parameter))
                : (parameter as ExcludedFoldersViewModel).SelectedFolder != null;
    }
}
