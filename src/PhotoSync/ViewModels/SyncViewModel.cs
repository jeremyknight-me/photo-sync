using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PhotoSync.Commands;
using PhotoSync.Data;
using PhotoSync.Data.Entities;
using PhotoSync.Models;
using PhotoSync.Windows;

namespace PhotoSync.ViewModels
{
    public class SyncViewModel : INotifyPropertyChanged
    {
        private bool isLoading = false;

        public SyncViewModel()
        {
            this.CloseCommand = new RelayCommand(
                paramater => {
                    var window = Application.Current.Windows.OfType<SyncWindow>().SingleOrDefault(x => x.IsActive);
                    window.Close();
                },
                parameter => !this.IsLoading
            );
            this.SyncCommand = new RelayCommand(
                parameter => {
                    this.IsLoading = true;
                    var library = AppState.Instance.Library;
                    if (library != null)
                    {
                        this.Logs.Add("Starting sync...");
                        this.RunSync(library);
                        this.Logs.Add("Sync completed...");
                    }
                    this.IsLoading = false;
                },
                parameter => !this.IsLoading
            );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SyncCommand { get; private set; } 
        public ICommand CloseCommand { get; private set; }

        public bool IsLoading
        {
            get => this.isLoading;
            set
            {
                if (this.isLoading != value)
                {
                    this.isLoading = value;
                    this.NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> Logs { get; set; } = new ObservableCollection<string>();

        private void RunSync(PhotoLibrary library)
        {
            this.DeleteOrphanedPhotos(library);
            var photos = this.GetSyncPhotos(library);
            var exceptions = new ConcurrentBag<Exception>();
            Parallel.ForEach(photos, photo =>
            {
                try
                {
                    var destinationPath = Path.Combine(library.DestinationFolder, photo.RelativePath);
                    var directory = Path.GetDirectoryName(destinationPath);
                    switch (photo.ProcessAction)
                    {
                        case var action when action == PhotoAction.Sync && !File.Exists(destinationPath):
                            var sourcePath = Path.Combine(library.SourceFolder, photo.RelativePath);
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            File.Copy(sourcePath, destinationPath, true);
                            this.Logs.Add($"Copied {photo.RelativePath}");
                            break;
                        case var action when action == PhotoAction.Ignore && File.Exists(destinationPath):
                            File.Delete(destinationPath);
                            if (!Directory.EnumerateFileSystemEntries(directory).Any())
                            {
                                Directory.Delete(directory);
                            }
                            this.Logs.Add($"Deleted {photo.RelativePath}");
                            break;
                        case PhotoAction.New:
                            // do nothing for new photos
                            break;
                        default:
                            throw new Exception("Invalid photo action");
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            });

            if (!exceptions.IsEmpty)
            {
                throw new AggregateException(exceptions);
            }
        }

        private IEnumerable<Photo> GetSyncPhotos(PhotoLibrary library)
        {
            using var context = PhotoSyncContextFactory.Make(library.DestinationFullPath);
            return context.Photos
                .Where(x => x.ProcessAction == PhotoAction.Ignore || x.ProcessAction == PhotoAction.Sync)
                .ToArray();
        }


        private void DeleteOrphanedPhotos(PhotoLibrary library)
        {
            var query = new GetFilesQuery();
            var sourcePathLength = library.SourceFolder.Length;
            var sourceFiles = query.Run(new DirectoryInfo(library.SourceFolder));
            var sourcePaths = sourceFiles.AsParallel().Select(x => x.FullName.Remove(0, sourcePathLength).TrimStart(new[] { '\\' }));
            var destinationPathLength = library.DestinationFolder.Length;
            var destinationFiles = query.Run(new DirectoryInfo(library.DestinationFolder));
            var destinationPaths = sourceFiles.AsParallel().Select(x => x.FullName.Remove(0, destinationPathLength).TrimStart(new[] { '\\' }));
            var orphanedPaths = destinationPaths.Except(sourcePaths).AsParallel();
            var exceptions = new ConcurrentBag<Exception>();
            Parallel.ForEach(orphanedPaths, orphanedPath =>
            {
                try
                {
                    File.Delete(Path.Combine(library.DestinationFolder, orphanedPath));
                    using var context = PhotoSyncContextFactory.Make(library.DestinationFullPath);
                    var photo = context.Photos.FirstOrDefault(x => x.RelativePath == orphanedPath);
                    if (photo != null)
                    {
                        context.Photos.Remove(photo);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            });

            if (!exceptions.IsEmpty)
            {
                throw new AggregateException(exceptions);
            }
        }

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
