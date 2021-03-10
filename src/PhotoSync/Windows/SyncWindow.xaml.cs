using System.Windows;
using PhotoSync.ViewModels;

namespace PhotoSync.Windows
{
    /// <summary>
    /// Interaction logic for SyncWindow.xaml
    /// </summary>
    public partial class SyncWindow : Window
    {
        public SyncWindow(SyncViewModel viewModel)
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
        }
    }
}
