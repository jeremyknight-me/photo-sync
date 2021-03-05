using System.Windows;
using PhotoSync.ViewModels;
using PhotoSync.Windows;

namespace PhotoSync
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainViewModel = new MainViewModel();
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}
