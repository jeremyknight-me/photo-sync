using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Data.Sqlite.Configuration.Converters;

internal sealed class PhotoIdValueConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<PhotoId, Guid>(id => id.Value, value => PhotoId.New(value), mappingHints)
{
    public PhotoIdValueConverter()
        : this(null)
    {
    }
}
