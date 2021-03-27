using System.Windows;
using System.Windows.Controls;
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

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                (this.DataContext as MainViewModel).SavePhoto(e.Row.Item as PhotoRecord);
            }
        }
    }
}
