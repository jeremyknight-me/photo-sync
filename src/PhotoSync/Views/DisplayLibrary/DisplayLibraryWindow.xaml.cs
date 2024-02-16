using System.Windows;

namespace PhotoSync.Views.DisplayLibrary;

public partial class DisplayLibraryWindow : Window
{
    public DisplayLibraryWindow(DisplayLibraryViewModel viewModel)
    {
        this.DataContext = viewModel;
        this.InitializeComponent();
    }

    public DisplayLibraryViewModel ViewModel => this.DataContext as DisplayLibraryViewModel;
}
