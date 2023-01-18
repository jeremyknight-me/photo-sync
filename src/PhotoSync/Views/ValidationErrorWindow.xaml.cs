using System.Collections.Generic;
using System.Windows;
using PhotoSync.ViewModels;

namespace PhotoSync.Views;

public partial class ValidationErrorWindow : Window
{
    public ValidationErrorWindow(IDictionary<string, IList<string>> errors)
    {
        var vm = new ValidationErrorViewModel(errors);
        this.DataContext = vm;
        this.InitializeComponent();
    }
}
