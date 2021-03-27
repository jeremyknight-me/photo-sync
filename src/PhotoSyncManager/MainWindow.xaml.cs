using System.Windows;
using PhotoSyncManager.ViewModels;

namespace PhotoSyncManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new MainViewModel();
            this.InitializeComponent();
        }
    }
}
