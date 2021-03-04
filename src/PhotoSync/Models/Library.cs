namespace PhotoSync.Models
{
    public class Library
    {
        public string DestinationFolder { get; set; }
        public string DestinationFullPath => System.IO.Path.Combine(this.DestinationFolder, this.FileName);
        public string FileName { get; set; }
        public string Name { get; set; }
        public string NameDisplay => string.IsNullOrWhiteSpace(this.Name) ? this.DestinationFullPath : this.Name;
        public string SourceFolder { get; set; }
    }
}
