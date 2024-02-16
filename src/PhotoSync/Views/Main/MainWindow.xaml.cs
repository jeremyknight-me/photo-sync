using System.Windows;

namespace PhotoSync.Views.Main;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        this.DataContext = viewModel;
        this.InitializeComponent();
    }
}
