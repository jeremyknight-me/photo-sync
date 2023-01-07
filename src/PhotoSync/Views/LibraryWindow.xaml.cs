using System.Windows;
using PhotoSync.ViewModels;

namespace PhotoSync.Views;

public partial class LibraryWindow : Window
{
    public LibraryWindow(LibraryViewModel viewModel)
    {
        this.DataContext = viewModel;
        this.InitializeComponent();
    }
}
