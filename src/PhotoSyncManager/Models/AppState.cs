using System;
using System.Windows;
using PhotoSync.Common;
using PhotoSync.Common;

namespace PhotoSyncManager.Models
{
    public sealed class AppState 
    {
        #region Singleton Pattern Implementation

        private static readonly Lazy<AppState> lazy = new(() => new AppState());

        private AppState()
        {
        }

        public static AppState Instance => lazy.Value;

        #endregion

        public PhotoLibrary Library
        {
            get => this.GetProperty<PhotoLibrary>("library");
            set => this.SetProperty("library", value);
        }

        public PhotoSyncContext Make(bool migrate = false) => PhotoSyncContextFactory.Make(this.Library.DestinationFullPath, migrate);

        private T GetProperty<T>(string key)
            => Application.Current.Properties[key] is null
                ? default
                : (T)Application.Current.Properties[key];

        private void SetProperty(string key, object value) => Application.Current.Properties[key] = value;
    }
}
