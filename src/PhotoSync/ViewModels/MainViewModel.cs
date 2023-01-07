using System;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PhotoSync.Views;

namespace PhotoSync.ViewModels;

public partial class MainViewModel
{
    private readonly IServiceProvider services;

    public MainViewModel(IServiceProvider serviceProvider)
    {
        this.services = serviceProvider;
    }

    [RelayCommand]
    private void OpenLibrary(Window currentWindow)
    {
        // todo: open library / refresh library / send to library

        var next = this.services.GetRequiredService<LibraryWindow>();
        next.Show();
        currentWindow.Close();
    }

    [RelayCommand]
    private void CreateLibrary(Window currentWindow)
    {
        var next = this.services.GetRequiredService<CreateLibraryWindow>();
        next.Show();
        currentWindow.Close();
    }
}
