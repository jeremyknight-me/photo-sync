using System.Windows;
using CommunityToolkit.Mvvm.Input;

namespace PhotoSync.ViewModels;

public partial class DisplayValidationErrorViewModel
{
    private readonly List<string> errors = [];

    public DisplayValidationErrorViewModel(IDictionary<string, IList<string>> errorDictionary)
    {
        foreach (var error in errorDictionary)
        {
            foreach (var item in error.Value)
            {
                this.errors.Add($"{error.Key}: {item}");
            }
        }
    }

    public IReadOnlyList<string> Errors => this.errors.AsReadOnly();

    [RelayCommand]
    private void Close(Window currentWindow) => currentWindow.Close();
}
