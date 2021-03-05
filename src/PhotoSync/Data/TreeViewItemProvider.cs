using System.Collections.Generic;
using System.IO;
using System.Linq;
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

                if (item.Children.Any())
                {
                    items.Add(item);
                }
            }

            var jpgs = directoryInfo.GetFiles("*.jpg", SearchOption.TopDirectoryOnly);
            var jpegs = directoryInfo.GetFiles("*.jpeg", SearchOption.TopDirectoryOnly);
            var pngs = directoryInfo.GetFiles("*.png", SearchOption.TopDirectoryOnly);
            foreach (var file in jpegs.Union(jpgs).Union(pngs))
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
