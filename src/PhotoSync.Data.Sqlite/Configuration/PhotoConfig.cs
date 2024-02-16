using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSync.Data.Sqlite.Configuration.Converters;
using PhotoSync.Domain.Entities;

namespace PhotoSync.Data.Sqlite.Configuration;

internal sealed class PhotoConfig : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.ToTable("Photos");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion(new PhotoIdValueConverter());
        builder.Property(x => x.ProcessAction).IsRequired();
        builder.Property(x => x.RelativePath).IsRequired();
        builder.Property(x => x.SizeBytes).IsRequired();

        builder.HasIndex(x => x.RelativePath).IsUnique();
    }
}
