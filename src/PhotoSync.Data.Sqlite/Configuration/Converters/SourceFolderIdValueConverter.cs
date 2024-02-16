using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Data.Sqlite.Configuration.Converters;

internal sealed class SourceFolderIdValueConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<SourceFolderId, Guid>(id => id.Value, value => SourceFolderId.New(value), mappingHints)
{
    public SourceFolderIdValueConverter()
        : this(null)
    {
    }
}
