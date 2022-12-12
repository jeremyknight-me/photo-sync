using System;
using System.Linq;
using System.Windows;
using PhotoSync.Common;
using PhotoSyncManager.Models;
using PhotoSyncManager.ViewModels;
using PhotoSyncManager.Windows;

namespace PhotoSyncManager.Commands.ExcludedFolders
{
    internal class SaveFoldersCommand : RelayCommand
    {
        public SaveFoldersCommand()
            : base(ExecuteMethod, CanExecuteMethod)
        {
        }

        private static void ExecuteMethod(object parameter)
        {
            if (!(parameter is ExcludedFoldersViewModel))
            {
                throw new ArgumentException("Parameter must be the correct view model type.", nameof(parameter));
            }

            var library = AppState.Instance.Library;
            var viewModel = parameter as ExcludedFoldersViewModel;
            using var context = PhotoSyncContextFactory.Make(library.DestinationFullPath);
            context.ExcludeFolders.RemoveRange(context.ExcludeFolders.ToArray());
            foreach (var folder in viewModel.Folders)
            {
                context.ExcludeFolders.Add(folder);
            }

            context.SaveChanges();
            viewModel.MainViewModel.ProcessLibrary(library);

            var window = Application.Current.Windows.OfType<ExcludedFoldersWindow>().SingleOrDefault(x => x.IsActive);
            window.Close();
        }

        private static bool CanExecuteMethod(object parameter) => true;
    }
}
