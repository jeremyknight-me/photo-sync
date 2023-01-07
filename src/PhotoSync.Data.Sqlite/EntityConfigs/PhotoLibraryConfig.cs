using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSync.Domain;

namespace PhotoSync.Data.Sqlite.EntityConfigs;

internal sealed class PhotoLibraryConfig : IEntityTypeConfiguration<PhotoLibrary>
{
    public void Configure(EntityTypeBuilder<PhotoLibrary> builder)
    {
        builder.ToTable("PhotoLibraries");
        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.FilePath);

        builder.Property(x => x.SourceFolder).IsRequired();

        builder.HasMany(x => x.ExcludedFolders).WithOne();
        builder.HasMany(x => x.Photos).WithOne();
    }
}
