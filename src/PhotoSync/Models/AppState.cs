using System;

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

        public string SelectedLibrary { get; set; }
    }
}
