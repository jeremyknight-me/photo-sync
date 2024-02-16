using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSync.Data.Sqlite.Configuration.Converters;
using PhotoSync.Domain.Entities;

namespace PhotoSync.Data.Sqlite.Configuration;

internal sealed class ExcludedFolderConfig : IEntityTypeConfiguration<ExcludedFolder>
{
    public void Configure(EntityTypeBuilder<ExcludedFolder> builder)
    {
        builder.ToTable("ExcludedFolders");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion(new ExcludedFolderIdValueConverter());
        builder.Property(x => x.RelativePath).IsRequired();

        builder.HasIndex(x => x.RelativePath).IsUnique();
    }
}
