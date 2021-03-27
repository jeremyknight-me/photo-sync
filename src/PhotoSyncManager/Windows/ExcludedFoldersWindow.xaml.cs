using System.Windows;
using PhotoSyncManager.ViewModels;

namespace PhotoSyncManager.Windows
{
    public partial class ExcludedFoldersWindow : Window
    {
        public ExcludedFoldersWindow(MainViewModel main)
        {
            this.DataContext = new ExcludedFoldersViewModel(main);
            this.InitializeComponent();
        }
    }
}
