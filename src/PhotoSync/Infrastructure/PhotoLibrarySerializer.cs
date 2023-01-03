using System.Text.Json;
using PhotoSync.Domain;

namespace PhotoSync.Infrastructure;

internal static class PhotoLibrarySerializer
{
    internal static string Serialize(PhotoLibrary library)
    {
        var options = new JsonSerializerOptions
        {
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.Strict,
            WriteIndented = true
        };
        return JsonSerializer.Serialize(library, options);
    }

    internal static PhotoLibrary Deserialize(string json)
    {
        var options = new JsonSerializerOptions
        {
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.Strict,
            ReadCommentHandling = JsonCommentHandling.Skip,
            PropertyNameCaseInsensitive = true
        };
        var library = JsonSerializer.Deserialize<PhotoLibrary>(json, options);
        return library is null
            ? throw new JsonException("Library could not be deserialized.")
            : library;
    }
}
