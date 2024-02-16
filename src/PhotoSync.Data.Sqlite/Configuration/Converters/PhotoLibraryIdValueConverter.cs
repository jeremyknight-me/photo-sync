using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Data.Sqlite.Configuration.Converters;

internal sealed class PhotoLibraryIdValueConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<PhotoLibraryId, Guid>(id => id.Value, value => PhotoLibraryId.New(value), mappingHints)
{
    public PhotoLibraryIdValueConverter()
        : this(null)
    {
    }
}
