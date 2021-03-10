using PhotoSync.Data.Entities;

namespace PhotoSyncManager.Models
{
    public class TreeViewFileItem : TreeViewItemBase
    {
        public override bool IsFile => true;
        public Photo Photo { get; internal set; }
    }
}
