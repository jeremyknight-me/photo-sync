using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PhotoSync.Common;
using PhotoSync.Data;
using PhotoSyncManager.Commands;
using PhotoSyncManager.Models;

namespace PhotoSyncManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool isProcessing = false;
        private string selectedLibrary;

        public MainViewModel()
        {
            this.PhotoActionOptions = new ObservableCollection<KeyValuePair<int, string>>(PhotoActionHelper.MakeEnumerable());
            this.PhotoRecords = new ObservableCollection<PhotoRecord>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CloseLibraryCommand { get; private set; } = new CloseLibraryCommand();
        public ICommand ExcludedFoldersCommand { get; private set; } = new ManageExcludedFoldersCommand();
        public ICommand ExitCommand { get; private set; } = new ShutdownCommand();
        public ICommand NewCommand { get; private set; } = new NewLibraryCommand();
        public ICommand OpenCommand { get; private set; } = new OpenLibraryCommand();
        public ICommand RefreshLibraryCommand { get; private set; } = new RefreshLibraryCommand();

        public ObservableCollection<KeyValuePair<int, string>> PhotoActionOptions { get; private set; }
        public ObservableCollection<PhotoRecord> PhotoRecords { get; private set; }

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

        public void SetLibrary(PhotoLibrary library)
        {
            this.StartProcessing();
            AppState.Instance.Library = library;
            this.SelectedLibrary = library.DestinationFolder;
            this.ProcessLibrary(library);
            this.StopProcessing();
        }

        public void ProcessLibrary(PhotoLibrary library)
        {
            var processor = new LibraryProcessor();
            var items = processor.Run(library);
            this.PhotoRecords.Clear();
            foreach (var item in items.OrderBy(x => x.RelativePath))
            {
                this.PhotoRecords.Add(item);
            }
        }

        public void CloseLibrary()
        {
            this.SelectedLibrary = null;
            this.PhotoRecords.Clear();
            AppState.Instance.Library = null;
        }

        private void StartProcessing() => this.IsProcessing = true;
        private void StopProcessing() => this.IsProcessing = false;

        //private void SavePhoto()
        //{
        //    if (AppState.Instance.Library is null)
        //    {
        //        return;
        //    }

        //    using var context = PhotoSyncContextFactory.Make(AppState.Instance.Library.DestinationFullPath);
        //    var photo = context.Photos.Find(this.selectedPhoto.Id);
        //    photo.ProcessAction = this.selectedPhoto.ProcessAction;
        //    context.SaveChanges();
        //}

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
