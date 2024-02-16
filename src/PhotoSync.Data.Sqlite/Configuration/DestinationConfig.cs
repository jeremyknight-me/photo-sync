using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSync.Data.Sqlite.Configuration.Converters;
using PhotoSync.Domain.Entities;

namespace PhotoSync.Data.Sqlite.Configuration;

internal sealed class DestinationConfig : IEntityTypeConfiguration<Destination>
{
    public void Configure(EntityTypeBuilder<Destination> builder)
    {
        builder.ToTable("Destinations");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion(new DestinationIdValueConverter());
        builder.Property(x => x.FullPath).IsRequired();

        builder.HasIndex(x => x.FullPath).IsUnique();
    }
}
