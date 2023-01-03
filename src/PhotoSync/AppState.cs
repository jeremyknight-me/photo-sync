using PhotoSync.Domain;

namespace PhotoSync;

internal sealed class AppState
{
    public PhotoLibrary? Library { get; set; }
}
