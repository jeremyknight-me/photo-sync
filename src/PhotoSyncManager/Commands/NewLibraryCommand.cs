using System;
using System.Windows.Forms;
using PhotoSync.Common;
using PhotoSync.Common.Entities;
using PhotoSyncManager.Models;
using PhotoSyncManager.ViewModels;

namespace PhotoSyncManager.Commands
{
    internal class NewLibraryCommand : RelayCommand
    {
        public NewLibraryCommand()
            : base(ExecuteMethod, CanExecuteMethod)
        {
        }

        private static void ExecuteMethod(object parameter)
        {
            if (!(parameter is MainViewModel))
            {
                throw new ArgumentException("Parameter must be the correct view model type.", nameof(parameter));
            }

            var source = GetFolderPath("Select source folder");
            if (string.IsNullOrWhiteSpace(source))
            {
                return;
            }

            var destination = GetFolderPath("Select destination folder");
            if (string.IsNullOrWhiteSpace(destination))
            {
                return;
            }

            var settings = new Settings
            {
                DestinationFolder = destination,
                SourceFolder = source
            };
            var library = PhotoLibraryConverter.Convert(settings);
            using var context = PhotoSyncContextFactory.Make(library.DestinationFullPath, true);
            context.Settings.Add(settings);
            context.SaveChanges();
            (parameter as MainViewModel).SetLibrary(library);
        }

        private static bool CanExecuteMethod(object parameter) => AppState.Instance.Library is null;

        private static string GetFolderPath(string description)
        {
            var dialog = new FolderBrowserDialog
            {
                Description = description,
                RootFolder = Environment.SpecialFolder.MyPictures,
                ShowNewFolderButton = false,
                UseDescriptionForTitle = true
            };
            var result = dialog.ShowDialog();
            return result == DialogResult.OK ? dialog.SelectedPath : string.Empty;
        }
    }
}
