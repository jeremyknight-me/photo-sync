using System.Linq;
using System.Windows;
using PhotoSyncManager.Windows;

namespace PhotoSyncManager.Commands.ExcludedFolders
{
    internal class CloseWindowCommand : RelayCommand
    {
        public CloseWindowCommand()
            : base(ExecuteMethod, CanExecuteMethod)
        {
        }

        private static void ExecuteMethod(object parameter)
        {
            var window = Application.Current.Windows.OfType<ExcludedFoldersWindow>().SingleOrDefault(x => x.IsActive);
            window.Close();
        }

        private static bool CanExecuteMethod(object parameter) => true;
    }
}
