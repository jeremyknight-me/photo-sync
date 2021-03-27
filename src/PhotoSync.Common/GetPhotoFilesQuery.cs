using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoSync.Common
{
    public class GetPhotoFilesQuery
    {
        private readonly IEnumerable<string> extensions = new string[] { ".jpg", ".jpeg", ".png" };

        public IEnumerable<FileInfo> Run(PhotoLibrary library)
        {
            if (!Directory.Exists(library.SourceFolder))
            {
                return Enumerable.Empty<FileInfo>();
            }

            var directory = new DirectoryInfo(library.SourceFolder);
            var files = directory.GetFiles("*", SearchOption.AllDirectories);
            return files
                .AsParallel()
                .Where(x => this.extensions.Contains(x.Extension, StringComparer.OrdinalIgnoreCase));
            ;
        }
    }
}
