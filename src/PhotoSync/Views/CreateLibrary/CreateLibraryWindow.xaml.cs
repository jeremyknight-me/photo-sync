using System.Windows;

namespace PhotoSync.Views.CreateLibrary;

public partial class CreateLibraryWindow : Window
{
    public CreateLibraryWindow(CreateLibraryViewModel viewModel)
    {
        this.DataContext = viewModel;
        this.InitializeComponent();
    }
}
