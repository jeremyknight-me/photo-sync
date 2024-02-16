using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSync.Data.Sqlite.Configuration.Converters;
using PhotoSync.Domain.Entities;

namespace PhotoSync.Data.Sqlite.Configuration;

internal sealed class PhotoLibraryConfig : IEntityTypeConfiguration<PhotoLibrary>
{
    public void Configure(EntityTypeBuilder<PhotoLibrary> builder)
    {
        builder.ToTable("PhotoLibraries");
        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.FilePath);

        builder.Property(x => x.Id).HasConversion(new PhotoLibraryIdValueConverter());
    }
}
