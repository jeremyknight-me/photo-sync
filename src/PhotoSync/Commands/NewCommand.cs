using System;
using PhotoSync.Models;
using PhotoSync.ViewModels;
using PhotoSync.Windows;

namespace PhotoSync.Commands
{
    internal class NewCommand : RelayCommand
    {
        public NewCommand()
            : base(ExecuteMethod, CanExecuteMethod)
        {
        }

        private static void ExecuteMethod(object parameter)
        {
            if (!(parameter is MainViewModel))
            {
                throw new ArgumentException("Parameter must be the correct view model type.", nameof(parameter));
            }

            var main = parameter as MainViewModel;
            var viewModel = new CreateLibraryViewModel(main);
            var window = new CreateLibraryWindow(viewModel);
            _ = window.ShowDialog();
        }

        private static bool CanExecuteMethod(object parameter) => AppState.Instance.Library is null;
    }
}
