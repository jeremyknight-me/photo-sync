using System.Windows;
using PhotoSync.ViewModels;

namespace PhotoSync;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        this.DataContext = viewModel;
        this.InitializeComponent();
    }
}
