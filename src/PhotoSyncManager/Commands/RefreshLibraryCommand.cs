using System;
using PhotoSyncManager.Models;
using PhotoSyncManager.ViewModels;

namespace PhotoSyncManager.Commands
{
    internal class RefreshLibraryCommand : RelayCommand
    {
        public RefreshLibraryCommand()
            : base(ExecuteMethod, CanExecuteMethod)
        {
        }

        private static void ExecuteMethod(object parameter)
        {
            if (!(parameter is MainViewModel))
            {
                throw new ArgumentException("Parameter must be the correct view model type.", nameof(parameter));
            }

            var library = AppState.Instance.Library;
            if (library != null)
            {
                (parameter as MainViewModel).ProcessLibrary(library);
            }
        }

        private static bool CanExecuteMethod(object parameter) => !(AppState.Instance.Library is null);
    }
}
