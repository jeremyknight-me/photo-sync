using Microsoft.Extensions.DependencyInjection;
using PhotoSync.Data.Sqlite;
using PhotoSync.Data.Sqlite.Repositories;
using PhotoSync.Domain.Abstractions;
using PhotoSync.Domain.Operations;
using PhotoSync.ViewModels;
using PhotoSync.Views;

namespace PhotoSync;

internal static class DependencyInjectionHelper
{
    internal static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        return services.BuildServiceProvider();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IGetPhotosOperation, GetPhotosOperation>();
        services.AddTransient<IRefreshLibraryOperation, RefreshLibraryOperation>();

        services.AddSingleton<PhotoSyncContextFactory>();
        services.AddTransient<IPhotoLibraryRepository, SqlitePhotoLibraryRepository>();

        services.AddTransient<LoadingWindow>();

        services.AddTransient<MainViewModel>();
        services.AddTransient<MainWindow>();

        services.AddTransient<CreateLibraryViewModel>();
        services.AddTransient<CreateLibraryWindow>();

        services.AddTransient<LibraryViewModel>();
        services.AddTransient<LibraryWindow>();
    }
}
