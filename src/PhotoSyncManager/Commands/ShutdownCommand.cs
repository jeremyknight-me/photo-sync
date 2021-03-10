using System.Windows;

namespace PhotoSyncManager.Commands
{
    internal class ShutdownCommand : RelayCommand
    {
        public ShutdownCommand()
            : base(obj => Application.Current.Shutdown())
        {
        }
    }
}
