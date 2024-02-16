using System.Windows;

namespace PhotoSync.Views.DisplaySourceFolder;
/// <summary>
/// Interaction logic for DisplaySourceFolderWindow.xaml
/// </summary>
public partial class DisplaySourceFolderWindow : Window
{
    public DisplaySourceFolderWindow(DisplaySourceFolderViewModel viewModel)
    {
        this.DataContext = viewModel;
        this.InitializeComponent();
    }

    public DisplaySourceFolderViewModel ViewModel => this.DataContext as DisplaySourceFolderViewModel;
}
