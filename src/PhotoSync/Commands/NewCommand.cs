using PhotoSync.Models;
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
            var window = new CreateLibraryWindow();
            window.ShowDialog();
        }

        private static bool CanExecuteMethod(object parameter) => AppState.Instance.Library is null;
    }
}
