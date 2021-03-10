using System.Collections.Generic;

namespace PhotoSyncManager.Models
{
    public class TreeViewDirectoryItem : TreeViewItemBase
    {
        public TreeViewDirectoryItem()
        {
            this.Children = new List<TreeViewItemBase>();
        }

        public List<TreeViewItemBase> Children { get; set; }

        public override bool IsFile => false;
    }
}
