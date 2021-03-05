namespace PhotoSync.Models
{
    public abstract class TreeViewItemBase
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public abstract bool IsFile { get; }
    }
}
