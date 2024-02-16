using System.Windows;
using PhotoSync.ViewModels;

namespace PhotoSync.Views.DisplayValidationError;

public partial class DisplayValidationErrorWindow : Window
{
    public DisplayValidationErrorWindow(IDictionary<string, IList<string>> errors)
    {
        var vm = new DisplayValidationErrorViewModel(errors);
        this.DataContext = vm;
        this.InitializeComponent();
    }
}
