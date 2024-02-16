using Microsoft.Extensions.DependencyInjection;
using PhotoSync.Data.Sqlite;
using PhotoSync.Data.Sqlite.Repositories;
using PhotoSync.Domain.Abstractions;
using PhotoSync.Domain.Operations;
using PhotoSync.Views;
using PhotoSync.Views.CreateLibrary;
using PhotoSync.Views.DisplayLibrary;
using PhotoSync.Views.Main;

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
        services.AddTransient<IRefreshSourceFolderOperation, RefreshSourceFolderOperation>();
        services.AddTransient<IRefreshLibraryOperation, RefreshLibraryOperation>();

        services.AddSingleton<PhotoSyncContextFactory>();
        services.AddTransient<IPhotoLibraryRepository, SqlitePhotoLibraryRepository>();

        services.AddTransient<LoadingWindow>();

        services.AddTransient<MainViewModel>();
        services.AddTransient<MainWindow>();

        services.AddTransient<CreateLibraryViewModel>();
        services.AddTransient<CreateLibraryWindow>();

        services.AddTransient<DisplayLibraryViewModel>();
        services.AddTransient<DisplayLibraryWindow>();
    }
}
