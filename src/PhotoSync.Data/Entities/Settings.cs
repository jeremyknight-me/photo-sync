using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PhotoSync.Data.Entities
{
    public class Settings
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string DestinationFolder { get; set; }
        public string SourceFolder { get; set; }
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
}
