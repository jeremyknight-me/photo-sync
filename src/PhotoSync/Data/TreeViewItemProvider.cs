using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PhotoSync.Models;

namespace PhotoSync.Data
{
    public class TreeViewItemProvider
    {
        public List<TreeViewItemBase> GetChildren(string path)
        {
            var items = new List<TreeViewItemBase>();
            var directoryInfo = new DirectoryInfo(path);
            var excludedDirectories = new string[] { "#recycle" };
            foreach (var directory in directoryInfo.GetDirectories().Where(x => !excludedDirectories.Contains(x.Name)))
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

            var bag = new ConcurrentBag<FileInfo>();
            var extensions = new string[] { "*.jpg", "*.jpeg", "*.png" };
            Parallel.ForEach(extensions, extension =>
            {
                var files = directoryInfo.GetFiles(extension, SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    bag.Add(file);
                }
            });

            foreach (var file in bag.OrderBy(x => x.FullName))
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
