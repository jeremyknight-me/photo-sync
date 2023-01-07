using System.Windows;
using PhotoSync.ViewModels;

namespace PhotoSync.Views
{
    public partial class CreateLibraryWindow : Window
    {
        public CreateLibraryWindow(CreateLibraryViewModel viewModel)
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
        }
    }
}
