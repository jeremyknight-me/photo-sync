using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PhotoSync.Commands;
using PhotoSync.Data;
using PhotoSync.Data.Entities;
using PhotoSync.Models;

namespace PhotoSync.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool isProcessing = false;
        private string selectedLibrary;
        private TreeViewItemBase selectedItem;
        private Photo selectedPhoto;
        private string selectedPhotoPath;
        private List<TreeViewItemBase> treeViewItems = new List<TreeViewItemBase>();

        public MainViewModel()
        {
            this.PhotoActionOptions = new ObservableCollection<KeyValuePair<int, string>>(PhotoActionHelper.MakeEnumerable());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CloseLibraryCommand { get; private set; } = new CloseLibraryCommand();
        public ICommand ExitCommand { get; private set; } = new ShutdownCommand();
        public ICommand NewCommand { get; private set; } = new NewLibraryCommand();
        public ICommand OpenCommand { get; private set; } = new OpenLibraryCommand();
        public ICommand RefreshLibraryCommand { get; private set; } = new RefreshLibraryCommand();
        
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

        public TreeViewItemBase SelectedItem
        {
            get => this.selectedItem;
            set
            {
                this.selectedItem = value;
                if (value is TreeViewFileItem)
                {
                    var item = value as TreeViewFileItem;
                    this.SelectedPhotoPath = item.Path;
                    this.SelectedPhoto = item.Photo;
                }
                else
                {
                    this.SelectedPhotoPath = string.Empty;
                    this.SelectedPhoto = null;
                }

                this.NotifyPropertyChanged();
            } 
        }

        public string SelectedPhotoPath
        {
            get => this.selectedPhotoPath;
            set
            {
                if (this.selectedPhotoPath != value)
                {
                    this.selectedPhotoPath = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public Photo SelectedPhoto
        {
            get => this.selectedPhoto;
            set
            {
                this.selectedPhoto = value;
                if (value != null)
                {
                    this.SelectedPhotoAction = (int)value.ProcessAction;
                }
                this.NotifyPropertyChanged();
            }
        }

        public int SelectedPhotoAction
        {
            get
            {
                var value = this.selectedPhoto;
                return value is null
                    ? 0
                    : (int)this.selectedPhoto.ProcessAction;
            }
            set
            {
                if (this.selectedPhoto != null)
                {
                    this.selectedPhoto.ProcessAction = (PhotoAction)value;
                    this.SavePhoto();
                    this.NotifyPropertyChanged();
                }
            }
        }

        public List<TreeViewItemBase> TreeViewItems
        {
            get => this.treeViewItems;
            set
            {
                this.treeViewItems = value;
                this.NotifyPropertyChanged();
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
            this.TreeViewItems = processor.Run(library);
        }

        public void CloseLibrary()
        {
            this.SelectedLibrary = null;
            AppState.Instance.Library = null;
        }

        private void StartProcessing() => this.IsProcessing = true;
        private void StopProcessing() => this.IsProcessing = false;

        private void SavePhoto()
        {
            if (AppState.Instance.Library is null)
            {
                return;
            }

            using var context = PhotoSyncContextFactory.Make(AppState.Instance.Library.DestinationFullPath);
            var photo = context.Photos.Find(this.selectedPhoto.Id);
            photo.ProcessAction = this.selectedPhoto.ProcessAction;
            context.SaveChanges();
        }

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
