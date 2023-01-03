using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PhotoSync.Infrastructure;

namespace PhotoSync;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddWpfBlazorWebView();
#if DEBUG
        serviceCollection.AddBlazorWebViewDeveloperTools();
#endif
        this.SetupDependencies(serviceCollection);
        this.Resources.Add("services", serviceCollection.BuildServiceProvider());
    }

    private void SetupDependencies(ServiceCollection services)
    {
        services.AddSingleton<AppState>();
        services.AddTransient<IPhotoLibraryRepository, JsonFilePhotoLibraryRepository>();
        //services.AddTransient<IPhotoLibraryRepository, CompressedPhotoLibraryRepository>();
    }
}
