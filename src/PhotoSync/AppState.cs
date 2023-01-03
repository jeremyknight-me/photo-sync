using System;
using System.Threading.Tasks;
using PhotoSync.Domain;

namespace PhotoSync;

internal sealed class AppState
{
    public PhotoLibrary? Library { get; set; }
    public Func<Task> HideLoadingModal { get; set; }
    public Func<Task> ShowLoadingModal { get; set; }
}
