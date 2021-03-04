using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PhotoSync.Commands;
using PhotoSync.Models;

namespace PhotoSync.ViewModels
{
    internal class MainViewModel
    {
        public MainViewModel()
        {
            this.ExitCommand = new ShutdownCommand();

            this.PhotoActionOptions = new ObservableCollection<KeyValuePair<int, string>>(PhotoActionHelper.MakeEnumerable());
        }

        public ICommand ExitCommand { get; private set; }

        public ObservableCollection<KeyValuePair<int, string>> PhotoActionOptions { get; set; }
        public int SelectedPhotoAction { get; set; }
    }
}
