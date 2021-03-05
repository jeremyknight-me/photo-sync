using System.Windows;
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
    }
}
