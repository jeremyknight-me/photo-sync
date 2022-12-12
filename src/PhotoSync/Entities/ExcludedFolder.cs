using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PhotoSync.Entities;

public class ExcludeFolder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string RelativePath { get; set; }
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
