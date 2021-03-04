using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PhotoSync.Commands;
using PhotoSync.Models;

namespace PhotoSync.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private string selectedLibrary;
        private string selectedPhoto;

        public MainViewModel()
        {
            this.ExitCommand = new ShutdownCommand();

            this.PhotoActionOptions = new ObservableCollection<KeyValuePair<int, string>>(PhotoActionHelper.MakeEnumerable());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ExitCommand { get; private set; }
        public ICommand NewCommand { get; private set; }

        public ObservableCollection<KeyValuePair<int, string>> PhotoActionOptions { get; private set; }

        public int SelectedPhotoAction { get; set; }

        public string SelectedLibrary
        {
            get => string.IsNullOrWhiteSpace(this.selectedLibrary)
                ? "No Sync Database Selected..."
                : this.selectedLibrary;
            private set
            {
                if (this.selectedLibrary != value)
                {
                    this.selectedLibrary = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public string SelectedPhoto
        {
            get => string.IsNullOrWhiteSpace(this.selectedPhoto)
                ? "No Photo Selected..."
                : this.selectedPhoto;
            private set
            {
                if (this.selectedPhoto != value)
                {
                    this.selectedPhoto = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
