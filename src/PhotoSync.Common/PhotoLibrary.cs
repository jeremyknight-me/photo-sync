namespace PhotoSync.Common
{
    public class PhotoLibrary
    {
        public string DestinationFolder { get; set; }
        public string DestinationFullPath => System.IO.Path.Combine(this.DestinationFolder, this.FileName);
        public string FileName { get; set; } = "photo-sync.db";
        public string SourceFolder { get; set; }
    }
}
