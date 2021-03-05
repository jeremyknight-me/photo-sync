using System.Collections.Generic;

namespace PhotoSync.Models
{
    public class TreeViewDirectoryItem : TreeViewItemBase
    {
        public TreeViewDirectoryItem()
        {
            this.Children = new List<TreeViewItemBase>();
        }

        public List<TreeViewItemBase> Children { get; set; }
    }
}
