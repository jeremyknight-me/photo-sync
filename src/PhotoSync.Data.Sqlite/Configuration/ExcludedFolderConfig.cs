﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PhotoSync.Domain.Entities;
using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Data.Sqlite.Configuration;

internal sealed class ExcludedFolderConfig : IEntityTypeConfiguration<ExcludedFolder>
{
    public void Configure(EntityTypeBuilder<ExcludedFolder> builder)
    {
        builder.ToTable("ExcludedFolders");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion(new ExcludedFolderId.EfCoreValueConverter());
        builder.Property(x => x.RelativePath).IsRequired();

        builder.HasIndex(x => x.RelativePath).IsUnique();
    }
}
