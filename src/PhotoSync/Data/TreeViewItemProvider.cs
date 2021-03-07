using System;
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

            var files = this.GetFiles(directoryInfo);
            foreach (var file in files.OrderBy(x => x.FullName))
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

        private IEnumerable<FileInfo> GetFiles(DirectoryInfo directoryInfo)
        {
            var extensions = new string[] { ".jpg", ".jpeg", ".png" };
            var files = directoryInfo.GetFiles("*", SearchOption.TopDirectoryOnly);
            return files
                .AsParallel()
                .Where(x => extensions.Contains(x.Extension, StringComparer.OrdinalIgnoreCase));
        }
    }
}
