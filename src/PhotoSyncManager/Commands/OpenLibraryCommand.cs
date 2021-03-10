using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PhotoSync.Data;
using PhotoSyncManager.Models;
using PhotoSyncManager.ViewModels;

namespace PhotoSyncManager.Commands
{
    internal class OpenLibraryCommand : RelayCommand
    {
        public OpenLibraryCommand()
            : base(ExecuteMethod, CanExecuteMethod)
        {
        }

        private static void ExecuteMethod(object parameter)
        {
            if (!(parameter is MainViewModel))
            {
                throw new ArgumentException("Parameter must be the correct view model type.", nameof(parameter));
            }

            var dialog = new OpenFileDialog
            {
                Title = "Open Library",
                Filter = "db files (*.db)|*.db|All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Multiselect = false
            };

            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var path = dialog.FileName;
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    using var context = PhotoSyncContextFactory.Make(path);
                    var settings = context.Settings.ToArray();
                    var library = PhotoLibraryConverter.Convert(settings);
                    (parameter as MainViewModel).SetLibrary(library);
                }
            }
        }

        private static bool CanExecuteMethod(object parameter) => AppState.Instance.Library is null;
    }
}
