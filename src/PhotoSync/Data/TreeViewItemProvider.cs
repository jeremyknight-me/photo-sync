using System.Collections.Generic;
using System.IO;
using PhotoSync.Models;

namespace PhotoSync.Data
{
    public class TreeViewItemProvider
    {
        public List<TreeViewItemBase> GetChildren(string path)
        {
            var items = new List<TreeViewItemBase>();
            var directoryInfo = new DirectoryInfo(path);

            foreach (var directory in directoryInfo.GetDirectories())
            {
                var item = new TreeViewDirectoryItem
                {
                    Name = directory.Name,
                    Path = directory.FullName,
                    Children = this.GetChildren(directory.FullName)
                };

                items.Add(item);
            }

            foreach (var file in directoryInfo.GetFiles())
            {
                var item = new TreeViewFileItem
                {
                    Name = file.Name,
                    Path = file.FullName
                };

                items.Add(item);
            }

            return items;
        }
    }
}
