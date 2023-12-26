using System;
using Microsoft.Extensions.DependencyInjection;
using PhotoSync.Data.Json;
using PhotoSync.Domain.Contracts;
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
        services.AddTransient<IPhotoLibraryRepository, JsonFilePhotoLibraryRepository>();

        services.AddTransient<LoadingWindow>();

        services.AddTransient<MainViewModel>();
        services.AddTransient<MainWindow>();

        services.AddTransient<CreateLibraryViewModel>();
        services.AddTransient<CreateLibraryWindow>();

        services.AddTransient<LibraryViewModel>();
        services.AddTransient<LibraryWindow>();
    }
}
