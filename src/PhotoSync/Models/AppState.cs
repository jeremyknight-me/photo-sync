using System;
using System.Windows;
using PhotoSync.Data;

namespace PhotoSync.Models
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

        public Library Library
        {
            get => this.GetProperty<Library>("library");
            set => this.SetProperty("library", value);
        }

        public PhotoSyncContext Make() => PhotoSyncContextFactory.Make(this.Library.DestinationFullPath);

        private T GetProperty<T>(string key)
            => Application.Current.Properties[key] is null
                ? default
                : (T)Application.Current.Properties[key];

        private void SetProperty(string key, object value) => Application.Current.Properties[key] = value;
    }
}
