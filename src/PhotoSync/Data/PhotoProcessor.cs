using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PhotoSync.Data.Entities;
using PhotoSync.Models;

namespace PhotoSync.Data
{
    public class PhotoProcessor
    {
        public void Run(PhotoLibrary library)
        {
            var relativePaths = new GetPhotoFilePathsQuery().Run(library.SourceFolder);
            var exceptions = new ConcurrentQueue<Exception>();
            Parallel.ForEach(relativePaths, relativePath =>
            {
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

            if (!exceptions.IsEmpty)
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}
