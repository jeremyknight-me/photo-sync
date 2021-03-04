using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;
using PhotoSync.Commands;
using PhotoSync.Data;
using PhotoSync.Models;
using PhotoSync.Windows;

namespace PhotoSync.ViewModels
{
    public class CreateLibraryViewModel : INotifyPropertyChanged
    {
        private string destinationFolder;
        private string fileName = "photo-sync.db";
        private string sourceFolder;

        public CreateLibraryViewModel(CreateLibraryWindow window)
        {
            this.CancelCommand = new RelayCommand(obj => window.Close());
            this.SaveCommand = new RelayCommand(
                obj =>
                {
                    this.CreateLibrary();
                    window.Close();
                },
                obj =>
                {
                    return !string.IsNullOrWhiteSpace(this.destinationFolder)
                        && !string.IsNullOrWhiteSpace(this.fileName)
                        && !string.IsNullOrWhiteSpace(this.sourceFolder);
                }
            );
            this.SelectDestinationCommand = new RelayCommand(obj =>
                this.RequestDirectoryFromUser(
                    () => this.destinationFolder,
                    path => this.SelectedDestinationFolder = path
                )
            );
            this.SelectSourceCommand = new RelayCommand(obj =>
                this.RequestDirectoryFromUser(
                    () => this.sourceFolder,
                    path => this.SelectedSourceFolder = path
                )
            );
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SelectDestinationCommand { get; private set; }
        public ICommand SelectSourceCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public string SelectedDestinationFolder
        {
            get => this.destinationFolder;
            set
            {
                if (value != this.destinationFolder)
                {
                    this.destinationFolder = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public string SelectedFileName
        {
            get => this.fileName;
            set
            {
                if (value != this.fileName)
                {
                    this.fileName = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public string SelectedSourceFolder
        {
            get => this.sourceFolder;
            set
            {
                if (value != this.sourceFolder)
                {
                    this.sourceFolder = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        private void RequestDirectoryFromUser(Func<string> getFolder, Action<string> setFolder)
        {
            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = false
            };
            if (!string.IsNullOrWhiteSpace(getFolder()))
            {
                dialog.SelectedPath = getFolder();
            }

            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var path = dialog.SelectedPath;
                if (!string.IsNullOrEmpty(path))
                {
                    setFolder(path);
                }
            }
            else if (result != DialogResult.Cancel)
            {
                setFolder(string.Empty);
            }
        }

        private void CreateLibrary()
        {
            var library = new Library
            {
                DestinationFolder = this.destinationFolder,
                FileName = this.fileName,
                SourceFolder = this.sourceFolder
            };

            using (var context = PhotoSyncContextFactory.Make(library.DestinationFullPath))
            {
                var settings = LibraryConverter.Convert(library);
                context.Settings.AddRange(settings);
                _ = context.SaveChanges();
            }

            AppState.Instance.Library = library;
        }

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
