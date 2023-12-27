using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSync.Domain.Entities;
using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Data.Sqlite.Configuration;

internal sealed class SourceFolderConfig : IEntityTypeConfiguration<SourceFolder>
{
    public void Configure(EntityTypeBuilder<SourceFolder> builder)
    {
        builder.ToTable("SourceFolders");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion(new SourceFolderId.EfCoreValueConverter());
        builder.Property(x => x.FullPath).IsRequired();

        builder.HasIndex(x => x.FullPath).IsUnique();

        builder
            .HasMany(s => s.ExcludedFolders)
            .WithOne(e => e.SourceFolder)
            .HasForeignKey(e => e.SourceFolderId);
        builder
            .HasMany(s => s.Photos)
            .WithOne(p => p.SourceFolder)
            .HasForeignKey(p => p.SourceFolderId);
    }
}
