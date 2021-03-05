using System;
using System.Collections.Generic;
using PhotoSync.Data.Entities;

namespace PhotoSync.Models
{
    public static class PhotoLibraryConverter
    {
        private const string destinationFolderKey = "destination_folder";
        private const string fileNameKey = "file_name";
        private const string sourceFolderKey = "source_folder";

        public static PhotoLibrary Convert(IList<Setting> settings)
        {
            var strategies = new Dictionary<string, Action<PhotoLibrary, Setting>>
            {
                { destinationFolderKey, (l, s) => l.DestinationFolder = s.Value },
                { fileNameKey, (l, s) => l.FileName = s.Value },
                { sourceFolderKey, (l, s) => l.SourceFolder = s.Value }
            };

            var library = new PhotoLibrary();
            foreach (var setting in settings)
            {
                if (strategies.ContainsKey(setting.Key))
                {
                    strategies[setting.Key](library, setting);
                }
            }

            return library;
        }

        public static IEnumerable<Setting> Convert(PhotoLibrary library)
            => new List<Setting>
            {
                new(destinationFolderKey, library.DestinationFolder),
                new(fileNameKey, library.FileName),
                new(sourceFolderKey, library.SourceFolder)
            };
    }
}
