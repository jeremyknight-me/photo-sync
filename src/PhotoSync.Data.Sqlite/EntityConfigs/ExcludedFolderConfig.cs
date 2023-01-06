using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoSync.Domain;

namespace PhotoSync.Data.Sqlite.EntityConfigs;

internal sealed class ExcludedFolderConfig : IEntityTypeConfiguration<ExcludedFolder>
{
    public void Configure(EntityTypeBuilder<ExcludedFolder> builder)
    {
        builder.ToTable("ExcludedFolders");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.RelativePath).IsRequired();
    }
}
