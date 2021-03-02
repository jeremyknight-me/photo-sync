using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PhotoSync.Data.Entities
{
    public class Photo
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string RelativePath { get; set; }
        public bool IsOrphaned { get; set; } = false;
        public bool IsNew { get; set; } = false;
        public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.Now;
    }

    internal class PhotoConfig : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.ToTable("Photo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RelativePath).IsRequired();

            builder.Property(x => x.DateCreated)
                .HasConversion(new DateTimeOffsetToStringConverter());
        }
    }
}
