using System;
using PhotoSync.Models;
using PhotoSync.ViewModels;

namespace PhotoSync.Commands
{
    internal class CloseLibraryCommand : RelayCommand
    {
        public CloseLibraryCommand()
            : base(ExecuteMethod, CanExecuteMethod)
        {
        }

        private static void ExecuteMethod(object parameter)
        {
            if (!(parameter is MainViewModel))
            {
                throw new ArgumentException("Parameter must be the correct view model type.", nameof(parameter));
            }

            (parameter as MainViewModel).CloseLibrary();
        }

        private static bool CanExecuteMethod(object parameter) => !(AppState.Instance.Library is null);
    }
}
