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

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var items = treeViewItemProvider.GetChildren(library.SourceFolder);
            watch.Stop();

            var fileItems = this.GetFiles(items);
            var files = new ConcurrentBag<TreeViewFileItem>();
            fileItems.ForEach(x => files.Add(x));
            var sourcePathLength = library.SourceFolder.Length;
            var exceptions = new ConcurrentQueue<Exception>();
            Parallel.ForEach(files, file =>
            {
                var relativePath = file.Path.Remove(0, sourcePathLength).TrimStart(new[] { '\\' });
                using var context = PhotoSyncContextFactory.Make(library.DestinationFullPath);
                if (context.Photos.Any(x => x.RelativePath == relativePath))
                {
                    var photo = context.Photos.FirstOrDefault(x => x.RelativePath == relativePath);
                    if (photo != null)
                    {
                        var sourceFilePath = Path.Combine(library.SourceFolder, relativePath);
                        if (!File.Exists(sourceFilePath))
                        {
                            context.Remove(photo);
                            context.SaveChanges();
                        }
                        else
                        {
                            file.Photo = photo;
                        }
                    }
                }
                else
                {
                    var photo = new Photo { RelativePath = relativePath };
                    context.Photos.Add(photo);
                    context.SaveChanges();
                    file.Photo = photo;
                }
            });

            return exceptions.IsEmpty
                ? items
                : throw new AggregateException(exceptions);
        }

        private List<TreeViewFileItem> GetFiles(IEnumerable<TreeViewItemBase> items)
        {
            var list = new List<TreeViewFileItem>();
            foreach (var directory in items.OfType<TreeViewDirectoryItem>())
            {
                var directoryFiles = this.GetFiles(directory.Children);
                list.AddRange(directoryFiles);
            }

            list.AddRange(items.OfType<TreeViewFileItem>());
            return list;
        }
    }
}
