using System.Windows;

namespace PhotoSync.Commands
{
    internal class ShutdownCommand : RelayCommand
    {
        public ShutdownCommand()
            : base(obj => Application.Current.Shutdown())
        {
        }
    }
}
