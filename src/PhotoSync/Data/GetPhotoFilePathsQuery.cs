using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSync.Data
{
    public class GetPhotoFilePathsQuery
    {
        public IEnumerable<string> Run(string rootDirectoryPath)
        {
            var files = Directory.EnumerateFiles(rootDirectoryPath, "*", SearchOption.AllDirectories);
            return this.GetPathsWithExtension(rootDirectoryPath, files);
        }

        private IEnumerable<string> GetPathsWithExtension(string rootDirectoryPath, IEnumerable<string> files)
        {
            var extensions = new HashSet<string> { ".jpg", ".jpeg", ".png" };
            var length = rootDirectoryPath.Length;
            var paths = new ConcurrentQueue<string>();
            var exceptions = new ConcurrentQueue<Exception>();
            Parallel.ForEach(files, file =>
            {
                try
                {
                    var fileExtension = Path.GetExtension(file);
                    if (extensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
                    {
                        var path = file.Remove(0, length).TrimStart(new[] { '\\' });
                        paths.Enqueue(path);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Enqueue(ex);
                }
            });

            return exceptions.IsEmpty
                ? paths
                : throw new AggregateException(exceptions);
        }
    }
}
