using System.Windows;
using PhotoSyncManager.Models;
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

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewItemBase)
            {
                (this.DataContext as MainViewModel).SelectedItem = e.NewValue as TreeViewItemBase;
            }
        }
    }
}
