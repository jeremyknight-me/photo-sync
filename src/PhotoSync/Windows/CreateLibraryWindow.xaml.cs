using System.Windows;
using PhotoSync.ViewModels;

namespace PhotoSync.Windows
{
    /// <summary>
    /// Interaction logic for CreateLibraryWindow.xaml
    /// </summary>
    public partial class CreateLibraryWindow : Window
    {
        public CreateLibraryWindow()
        {
            this.DataContext = new CreateLibraryViewModel(this);
            this.InitializeComponent();
        }
    }
}
