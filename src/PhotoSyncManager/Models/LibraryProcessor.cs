using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PhotoSync.Common;
using PhotoSync.Common;
using PhotoSync.Common.Entities;

namespace PhotoSyncManager.Models
{
    public class LibraryProcessor
    {
        public IEnumerable<PhotoRecord> Run(PhotoLibrary library)
        {
            var files = new GetPhotoFilesQuery().Run(library);
            var sourcePathLength = library.SourceFolder.Length;
            var records = new ConcurrentBag<PhotoRecord>();
            var exceptions = new ConcurrentBag<Exception>();

            Parallel.ForEach(files, file =>
            {
                try
                {
                    var relativePath = file.FullName.Remove(0, sourcePathLength).TrimStart(new[] { '\\' });
                    using var context = PhotoSyncContextFactory.Make(library.DestinationFullPath);
                    var excludedFolders = context.ExcludeFolders.Select(x => x.RelativePath);
                    if (!this.IsInExcludedFolder(excludedFolders, relativePath))
                    {
                        if (context.Photos.Any(x => x.RelativePath == relativePath))
                        {
                            var photo = context.Photos.FirstOrDefault(x => x.RelativePath == relativePath);
                            if (photo != null)
                            {
                                var sourceFilePath = Path.Combine(library.SourceFolder, relativePath);
                                if (!File.Exists(sourceFilePath))
                                {
                                    context.Photos.Remove(photo);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    records.Add(this.MakeRecord(photo, file));
                                }
                            }
                        }
                        else
                        {
                            var photo = new Photo { RelativePath = relativePath };
                            context.Photos.Add(photo);
                            context.SaveChanges();
                            records.Add(this.MakeRecord(photo, file));
                        }
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            });

            return exceptions.IsEmpty
                ? records
                : throw new AggregateException(exceptions);
        }

        private bool IsInExcludedFolder(IEnumerable<string> excludedFolders, string relativePath)
        {
            foreach (var folder in excludedFolders)
            {
                if (relativePath.StartsWith(folder))
                {
                    return true;
                }
            }

            return false;
        }

        private PhotoRecord MakeRecord(Photo photo, FileInfo file)
            => new PhotoRecord(photo)
            {
                FileName = file.Name,
                FullPath = file.FullName
            };
    }
}
