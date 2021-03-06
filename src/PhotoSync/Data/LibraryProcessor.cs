using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PhotoSync.Data.Entities;
using PhotoSync.Models;

namespace PhotoSync.Data
{
    public class LibraryProcessor
    {
        public List<TreeViewItemBase> Run(PhotoLibrary library)
        {
            var treeViewItemProvider = new TreeViewItemProvider();
            var items = treeViewItemProvider.GetChildren(library.SourceFolder);
            var paths = this.GetFiles(items);
            var sourcePathLength = library.SourceFolder.Length;
            var exceptions = new ConcurrentQueue<Exception>();
            Parallel.ForEach(paths, path =>
            {
                var relativePath = path.Remove(0, sourcePathLength).TrimStart(new[] { '\\' });
                using var context = PhotoSyncContextFactory.Make(library.DestinationFullPath);
                if (context.Photos.Any(x => x.RelativePath == relativePath))
                {
                    var sourceFilePath = Path.Combine(library.SourceFolder, relativePath);
                    if (!File.Exists(sourceFilePath))
                    {
                        var photo = context.Photos.FirstOrDefault(x => x.RelativePath == relativePath);
                        if (photo != null)
                        {
                            context.Remove(photo);
                            context.SaveChanges();
                        }
                    }
                }
                else
                {
                    var photo = new Photo { RelativePath = relativePath };
                    context.Photos.Add(photo);
                    context.SaveChanges();
                }
            });

            return exceptions.IsEmpty
                ? items
                : throw new AggregateException(exceptions);
        }

        private List<string> GetFiles(IEnumerable<TreeViewItemBase> items)
        {
            var list = new List<string>();
            foreach (var directory in items.OfType<TreeViewDirectoryItem>())
            {
                var directoryFiles = this.GetFiles(directory.Children);
                list.AddRange(directoryFiles);
            }

            list.AddRange(items.OfType<TreeViewFileItem>().Select(x => x.Path));
            return list;
        }
    }
}
