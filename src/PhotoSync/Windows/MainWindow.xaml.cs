using System.Windows;
using PhotoSync.Models;
using PhotoSync.ViewModels;

namespace PhotoSync.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            this.DataContext = mainViewModel;
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
