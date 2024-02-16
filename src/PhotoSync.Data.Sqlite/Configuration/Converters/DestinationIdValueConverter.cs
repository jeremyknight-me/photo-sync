using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhotoSync.Domain.ValueObjects;

namespace PhotoSync.Data.Sqlite.Configuration.Converters;

internal sealed class DestinationIdValueConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<DestinationId, Guid>(id => id.Value, value => DestinationId.New(value), mappingHints)
{
    public DestinationIdValueConverter()
        : this(null)
    {
    }
}
