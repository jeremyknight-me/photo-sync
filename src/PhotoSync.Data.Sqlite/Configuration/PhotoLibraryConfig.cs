using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhotoSync.Domain.Entities;
using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Data.Sqlite.Configuration;

internal sealed class PhotoLibraryConfig : IEntityTypeConfiguration<PhotoLibrary>
{
    public void Configure(EntityTypeBuilder<PhotoLibrary> builder)
    {
        builder.ToTable("PhotoLibraries");
        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.FilePath);

        builder.Property(x => x.Id).HasConversion(new PhotoLibraryId.EfCoreValueConverter());
    }
}
