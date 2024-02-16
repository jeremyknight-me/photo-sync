using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Data.Sqlite.Configuration.Converters;

internal sealed class ExcludedFolderIdValueConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<ExcludedFolderId, Guid>(id => id.Value, value => ExcludedFolderId.New(value), mappingHints)
{
    public ExcludedFolderIdValueConverter()
        : this(null)
    {
    }
}
