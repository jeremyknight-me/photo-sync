using PhotoSync.Common;
using PhotoSync.Data.Entities;

namespace PhotoSync.Data
{
    public static class PhotoLibraryConverter
    {
        public static PhotoLibrary Convert(Settings settings)
        {
            var library = new PhotoLibrary
            {
                DestinationFolder = settings.DestinationFolder,
                SourceFolder = settings.SourceFolder,
            };
            return library;
        }

        public static Settings Convert(PhotoLibrary library)
            => new Settings
            {
                DestinationFolder = library.DestinationFolder,
                SourceFolder = library.SourceFolder
            };
    }
}
