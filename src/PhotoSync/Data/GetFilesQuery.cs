using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoSync.Data
{
    public class GetFilesQuery
    {
        public IEnumerable<FileInfo> Run(DirectoryInfo directoryInfo)
        {
            var extensions = new string[] { ".jpg", ".jpeg", ".png" };
            var files = directoryInfo.GetFiles("*", SearchOption.TopDirectoryOnly);
            return files
                .AsParallel()
                .Where(x => extensions.Contains(x.Extension, StringComparer.OrdinalIgnoreCase));
        }
    }
}
