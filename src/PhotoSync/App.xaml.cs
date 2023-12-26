using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace PhotoSync;

public partial class App : System.Windows.Application
{
    private readonly IServiceProvider serviceProvider;

    public App()
    {
        this.serviceProvider = DependencyInjectionHelper.CreateServiceProvider();
    }

    //public new static App Current => (App)Application.Current;

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var mainWindow = this.serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}
