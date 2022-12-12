using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhotoSync.Enums;

namespace PhotoSync.Entities;

public class Photo
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public required string RelativePath { get; set; }
    public PhotoAction ProcessAction { get; set; } = PhotoAction.New;
}

internal class PhotoConfig : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.ToTable("Photos");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RelativePath).IsRequired();
    }
}
