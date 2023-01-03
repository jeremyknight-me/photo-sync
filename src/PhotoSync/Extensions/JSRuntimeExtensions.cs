using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace PhotoSync.Extensions;

internal static class JSRuntimeExtensions
{
    public async static Task BootstrapModalOpen(this IJSRuntime js, string elementId) => await js.InvokeVoidAsync("app.bootstrap.modal.open", elementId);
    public async static Task BootstrapModalClose(this IJSRuntime js, string elementId) => await js.InvokeVoidAsync("app.bootstrap.modal.close", elementId);
    public async static Task BootstrapModalInit(this IJSRuntime js, string elementId, bool backdrop = true, bool focus = true, bool keyboard = false, bool staticBackdrop = false)
    {
        object backdropSettings = staticBackdrop ? "static" : backdrop;
        await js.InvokeVoidAsync("app.bootstrap.modal.init", elementId, backdropSettings, focus, keyboard);
    }
}
