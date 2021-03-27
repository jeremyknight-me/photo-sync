using System.Windows;
using PhotoSyncManager.ViewModels;

namespace PhotoSyncManager.Windows
{
    public partial class ExcludedFoldersWindow : Window
    {
        public ExcludedFoldersWindow(ExcludedFoldersViewModel viewModel)
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
        }
    }
}
