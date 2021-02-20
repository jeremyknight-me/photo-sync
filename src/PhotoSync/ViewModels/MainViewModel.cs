using System.Windows.Input;
using PhotoSync.Commands;

namespace PhotoSync.ViewModels
{
    internal class MainViewModel
    {
        public MainViewModel()
        {
            this.ExitCommand = new ShutdownCommand();
        }

        public ICommand ExitCommand { get; private set; }
    }
}
