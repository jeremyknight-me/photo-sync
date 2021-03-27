using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoSync.Common
{
    public class GetPhotoFilesQuery
    {
        private readonly IEnumerable<string> extensions = new string[] { ".jpg", ".jpeg", ".png" };

        public IEnumerable<FileInfo> Run(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return Enumerable.Empty<FileInfo>();
            }

            var directory = new DirectoryInfo(directoryPath);
            var files = directory.GetFiles("*", SearchOption.AllDirectories);
            return files
                .AsParallel()
                .Where(x =>
                    this.extensions.Contains(x.Extension, StringComparer.OrdinalIgnoreCase)
                    && !x.FullName.Contains("\\#recycle\\")
                );
            ;
        }
    }
}
