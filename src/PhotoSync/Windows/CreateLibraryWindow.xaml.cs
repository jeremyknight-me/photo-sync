using System.Windows;
using PhotoSync.ViewModels;

namespace PhotoSync.Windows
{
    /// <summary>
    /// Interaction logic for CreateLibraryWindow.xaml
    /// </summary>
    public partial class CreateLibraryWindow : Window
    {
        public CreateLibraryWindow(CreateLibraryViewModel viewModel)
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
        }
    }
}
