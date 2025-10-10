using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Services;
using AutoAuction_H2.Views;
using AutoAuction_H2.Views.ContentPanels;
using AutoAuction_H2.Views.Windows;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels;

public partial class UserProfileViewModel : ViewModelBase
{
    [ObservableProperty] private string userName;
    [ObservableProperty] private decimal balance;

    private readonly INavigationService _navigation;

    public IRelayCommand LogoutCommand { get; }
    public IAsyncRelayCommand OpenProfileCommand { get; }

    public UserProfileViewModel(INavigationService navigation)
    {
        _navigation = navigation;

        LogoutCommand = new RelayCommand(DoLogout);
        OpenProfileCommand = new AsyncRelayCommand(DoOpenProfile);

        var app = AppState.Instance;
        userName = app.UserName ?? "Ukendt";
        balance = app.Balance;

        app.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(AppState.UserName))
                UserName = app.UserName;
            if (e.PropertyName == nameof(AppState.Balance))
                Balance = app.Balance;
        };
    }

    private void DoLogout()
    {
        var app = AppState.Instance;
        app.UserId = 0;
        app.UserName = "";
        app.Balance = 0;
        app.UserType = 0;

        // Tell MainWindow to swap back to login
        MainWindow.Current?.ShowLoginView();
    }



    private async Task DoOpenProfile()
    {
        var window = App.Services.GetRequiredService<UserProfileWindow>();

        if (App.Current?.ApplicationLifetime
            is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            && desktop.MainWindow is Window owner)
        {
            await window.ShowDialog(owner);
        }
        else
        {
            window.Show();
        }
    }
}
