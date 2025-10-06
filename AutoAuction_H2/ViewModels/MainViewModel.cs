using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AutoAuction_H2.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;
        private readonly IServiceProvider _provider;

        public MainViewModel(INavigationService navigation, IServiceProvider provider)
        {
            _navigation = navigation;
            _provider = provider;

            // Commands henter VM’er fra DI container
            ShowHomeCommand = new RelayCommand(() =>
                _navigation.NavigateTo(_provider.GetRequiredService<HomeScreenViewModel>()));

            ShowAuctionOverviewCommand = new RelayCommand(() =>
                _navigation.NavigateTo(_provider.GetRequiredService<AuctionOverviewViewModel>()));

            ShowPrivateCarsCommand = new RelayCommand(() =>
                _navigation.NavigateTo(_provider.GetRequiredService<PrivateCarsViewModel>()));

            ShowProfessionalCarsCommand = new RelayCommand(() =>
                _navigation.NavigateTo(_provider.GetRequiredService<ProfessionalCarsViewModel>()));

            ShowTrucksCommand = new RelayCommand(() =>
                _navigation.NavigateTo(_provider.GetRequiredService<TrucksViewModel>()));

            ShowBusesCommand = new RelayCommand(() =>
                _navigation.NavigateTo(_provider.GetRequiredService<BusesViewModel>()));

            ShowMyBidsCommand = new RelayCommand(() =>
                _navigation.NavigateTo(_provider.GetRequiredService<MyBidsViewModel>()));

            ShowMySalesCommand = new RelayCommand(() =>
                _navigation.NavigateTo(_provider.GetRequiredService<MySalesViewModel>()));

            ShowCreateAuctionCommand = new RelayCommand(() =>
                _navigation.NavigateTo(_provider.GetRequiredService<CreateAuctionViewModel>()));

            // Start på Home
            _navigation.NavigateTo(_provider.GetRequiredService<HomeScreenViewModel>());
        }

        // Commands
        public IRelayCommand ShowHomeCommand { get; }
        public IRelayCommand ShowAuctionOverviewCommand { get; }
        public IRelayCommand ShowPrivateCarsCommand { get; }
        public IRelayCommand ShowProfessionalCarsCommand { get; }
        public IRelayCommand ShowTrucksCommand { get; }
        public IRelayCommand ShowBusesCommand { get; }
        public IRelayCommand ShowMyBidsCommand { get; }
        public IRelayCommand ShowMySalesCommand { get; }
        public IRelayCommand ShowCreateAuctionCommand { get; }

        // Binding til ContentPresenter
        public ViewModelBase CurrentContent => _navigation.CurrentViewModel;
    }
}
