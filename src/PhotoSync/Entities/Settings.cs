using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace PhotoSync.Entities;

public class Settings
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public required string DestinationFolder { get; set; }
    public required string SourceFolder { get; set; }
}

internal class SettingsConfig : IEntityTypeConfiguration<Settings>
{
    public void Configure(EntityTypeBuilder<Settings> builder)
    {
        builder.ToTable("Settings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DestinationFolder).IsRequired();
        builder.Property(x => x.SourceFolder).IsRequired();
    }
}
