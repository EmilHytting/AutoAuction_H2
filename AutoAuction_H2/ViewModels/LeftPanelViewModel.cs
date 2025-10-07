using AutoAuction_H2.Services;
using AutoAuction_H2.Views.Windows;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AutoAuction_H2.ViewModels
{
    public class LeftPanelViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;
        private readonly IServiceProvider _services;

        public LeftPanelViewModel(INavigationService navigation, IServiceProvider services)
        {
            _navigation = navigation;
            _services = services;

            ShowHomeCommand = new RelayCommand(() => _navigation.NavigateTo<HomeScreenViewModel>());
            ShowAuctionOverviewCommand = new RelayCommand(() => _navigation.NavigateTo<AuctionOverviewViewModel>());
            ShowPrivateCarsCommand = new RelayCommand(() => _navigation.NavigateTo<PrivateCarsViewModel>());
            ShowProfessionalCarsCommand = new RelayCommand(() => _navigation.NavigateTo<ProfessionalCarsViewModel>());
            ShowTrucksCommand = new RelayCommand(() => _navigation.NavigateTo<TrucksViewModel>());
            ShowBusesCommand = new RelayCommand(() => _navigation.NavigateTo<BusesViewModel>());
            ShowMyBidsCommand = new RelayCommand(() => _navigation.NavigateTo<MyBidsViewModel>());
            ShowMySalesCommand = new RelayCommand(() => _navigation.NavigateTo<MySalesViewModel>());

            ShowCreateAuctionCommand = new RelayCommand(OpenCreateAuctionWindow);
        }

        public IRelayCommand ShowHomeCommand { get; }
        public IRelayCommand ShowAuctionOverviewCommand { get; }
        public IRelayCommand ShowPrivateCarsCommand { get; }
        public IRelayCommand ShowProfessionalCarsCommand { get; }
        public IRelayCommand ShowTrucksCommand { get; }
        public IRelayCommand ShowBusesCommand { get; }
        public IRelayCommand ShowMyBidsCommand { get; }
        public IRelayCommand ShowMySalesCommand { get; }
        public IRelayCommand ShowCreateAuctionCommand { get; }

        private void OpenCreateAuctionWindow()
        {
            var window = _services.GetRequiredService<CreateAuctionWindow>();

            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                && desktop.MainWindow is Window owner)
            {
                window.ShowDialog(owner);
            }
            else
            {
                window.Show();
            }
        }
    }
}
