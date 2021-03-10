using PhotoSync.Models;
using PhotoSync.ViewModels;
using PhotoSync.Windows;

namespace PhotoSync.Commands
{
    internal class SyncLibraryCommand : RelayCommand
    {
        public SyncLibraryCommand()
            : base(ExecuteMethod, CanExecuteMethod)
        {
        }

        private static void ExecuteMethod(object parameter)
        {
            var syncViewModel = new SyncViewModel();
            var syncWindow = new SyncWindow(syncViewModel);
            syncWindow.ShowDialog();
        }

        private static bool CanExecuteMethod(object parameter) => !(AppState.Instance.Library is null);
    }
}
