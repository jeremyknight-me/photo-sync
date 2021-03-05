using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PhotoSync.Commands;
using PhotoSync.Models;

namespace PhotoSync.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool isProcessing = false;
        private string selectedLibrary;

        public MainViewModel()
        {
            this.ExitCommand = new ShutdownCommand();
            this.NewCommand = new NewCommand();
            this.OpenCommand = new OpenCommand();
            this.PhotoActionOptions = new ObservableCollection<KeyValuePair<int, string>>(PhotoActionHelper.MakeEnumerable());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ExitCommand { get; private set; }
        public ICommand NewCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ObservableCollection<KeyValuePair<int, string>> PhotoActionOptions { get; private set; }

        public bool IsProcessing
        {
            get => this.isProcessing;
            private set
            {
                if (this.isProcessing != value)
                {
                    this.isProcessing = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public int SelectedPhotoAction { get; set; }

        public string SelectedLibrary
        {
            get => string.IsNullOrWhiteSpace(this.selectedLibrary)
                ? "Library: None Selected..."
                : $"Library: {this.selectedLibrary}";
            private set
            {
                if (this.selectedLibrary != value)
                {
                    this.selectedLibrary = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public void SetLibrary(Library library)
        {
            this.StartProcessing();
            AppState.Instance.Library = library;
            this.SelectedLibrary = library.DestinationFolder;
            this.StopProcessing();
        }

        private void StartProcessing() => this.IsProcessing = true;
        private void StopProcessing() => this.IsProcessing = false;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
