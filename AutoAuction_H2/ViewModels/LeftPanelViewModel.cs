using CommunityToolkit.Mvvm.Input;
using AutoAuction_H2.Services;

namespace AutoAuction_H2.ViewModels
{
    public class LeftPanelViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;
        private readonly AuctionService _auctionService;

        public LeftPanelViewModel(INavigationService navigation, AuctionService auctionService)
        {
            _navigation = navigation;
            _auctionService = auctionService;

            ShowHomeCommand = new RelayCommand(() =>
                _navigation.NavigateTo(new HomeScreenViewModel(_auctionService)));

            ShowAuctionOverviewCommand = new RelayCommand(() =>
                _navigation.NavigateTo(new AuctionOverviewViewModel(_auctionService)));

            //ShowPrivateCarsCommand = new RelayCommand(() =>
            //    _navigation.NavigateTo(new PrivateCarsViewModel(_auctionService)));

            //ShowTrucksCommand = new RelayCommand(() =>
            //    _navigation.NavigateTo(new TrucksViewModel(_auctionService)));

            //ShowBusesCommand = new RelayCommand(() =>
            //    _navigation.NavigateTo(new BusesViewModel(_auctionService)));

            //ShowMyBidsCommand = new RelayCommand(() =>
            //    _navigation.NavigateTo(new MyBidsViewModel(_auctionService)));

            //ShowMySalesCommand = new RelayCommand(() =>
            //    _navigation.NavigateTo(new MySalesViewModel(_auctionService)));

            //ShowCreateAuctionCommand = new RelayCommand(() =>
            //    _navigation.NavigateTo(new CreateAuctionViewModel(_auctionService)));
        }

        public IRelayCommand ShowHomeCommand { get; }
        public IRelayCommand ShowAuctionOverviewCommand { get; }
        public IRelayCommand ShowPrivateCarsCommand { get; }
        public IRelayCommand ShowTrucksCommand { get; }
        public IRelayCommand ShowBusesCommand { get; }
        public IRelayCommand ShowMyBidsCommand { get; }
        public IRelayCommand ShowMySalesCommand { get; }
        public IRelayCommand ShowCreateAuctionCommand { get; }
    }
}
