using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

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

    internal T GetService<T>() => this.serviceProvider.GetService<T>();

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var mainWindow = this.serviceProvider.GetService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
    }
}
