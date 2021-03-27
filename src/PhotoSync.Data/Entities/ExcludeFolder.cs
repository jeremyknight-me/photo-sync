using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PhotoSync.Data.Entities
{
    public class ExcludeFolder
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RelativePath { get; set; }
    }

    internal class ExcludeFolderConfig : IEntityTypeConfiguration<ExcludeFolder>
    {
        public void Configure(EntityTypeBuilder<ExcludeFolder> builder)
        {
            builder.ToTable("ExcludeFolders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.RelativePath).IsRequired();
        }
    }
}
