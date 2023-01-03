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
        services.AddSingleton<IPhotoLibraryRepository, JsonFilePhotoLibraryRepository>();
        //services.AddSingleton<IPhotoLibraryRepository, CompressedPhotoLibraryRepository>();
        services.AddSingleton<IGetPhotosQuery, GetPhotosQuery>();
        services.AddSingleton<IRefreshLibraryCommand, RefreshLibraryCommand>();
    }
}
