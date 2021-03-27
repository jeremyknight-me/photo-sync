using System;
using PhotoSync.Data;
using PhotoSync.Data.Entities;

namespace PhotoSyncManager.Models
{
    public class PhotoRecord
    {
        public PhotoRecord()
        {
        }

        public PhotoRecord(Photo photo)
        {
            this.Id = photo.Id;
            this.RelativePath = photo.RelativePath;
            this.ProcessAction = photo.ProcessAction;
        }

        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public string RelativePath { get; set; }
        public PhotoAction ProcessAction { get; set; }
    }
}
