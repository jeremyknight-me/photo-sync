using System;
using PhotoSyncManager.Models;
using PhotoSyncManager.ViewModels;
using PhotoSyncManager.Windows;

namespace PhotoSyncManager.Commands
{
    internal class ManageExcludedFoldersCommand : RelayCommand
    {
        public ManageExcludedFoldersCommand()
            : base(ExecuteMethod, CanExecuteMethod)
        {
        }

        private static void ExecuteMethod(object parameter)
        {
            if (!(parameter is MainViewModel))
            {
                throw new ArgumentException("Parameter must be the correct view model type.", nameof(parameter));
            }

            var mainViewModel = parameter as MainViewModel;
            var window = new ExcludedFoldersWindow(mainViewModel);
            window.Show();
        }

        private static bool CanExecuteMethod(object parameter) => AppState.Instance.Library is not null;
    }
}
