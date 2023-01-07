using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PhotoSync.Data.Sqlite;
using PhotoSync.Data.Sqlite.Repositories;
using PhotoSync.Domain.Contracts;
using PhotoSync.Domain.Operations;
using PhotoSync.ViewModels;
using PhotoSync.Views;

namespace PhotoSync;

public partial class App : Application
{
    private readonly ServiceProvider serviceProvider;

    public App()
    {
        var serviceCollection = new ServiceCollection();
        this.ConfigureServices(serviceCollection);
        this.serviceProvider = serviceCollection.BuildServiceProvider();
    }

    //public new static App Current => (App)Application.Current;

    internal T GetService<T>() => this.serviceProvider.GetService<T>();

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var mainWindow = this.serviceProvider.GetService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IGetPhotosOperation, GetPhotosOperation>();
        services.AddTransient<IRefreshLibraryOperation, RefreshLibraryOperation>();
        services.AddTransient<IPhotoLibraryRepository, SqlitePhotoLibraryRepository>();

        services.AddSingleton<PhotoSyncContextFactory>();

        services.AddTransient<MainViewModel>();
        services.AddTransient<MainWindow>();

        services.AddTransient<CreateLibraryViewModel>();
        services.AddTransient<CreateLibraryWindow>();

        services.AddTransient<LibraryViewModel>();
        services.AddTransient<LibraryWindow>();
    }
}
