using AutoAuction_H2.Services;
using AutoAuction_H2.ViewModels.Auction;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AutoAuction_H2.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;

        [ObservableProperty]
        private ViewModelBase? currentContent;

        public IRelayCommand ShowHomeCommand { get; }
        public IRelayCommand ShowAuctionOverviewCommand { get; }
        public IRelayCommand<AuctionDetailViewModel> ShowAuctionDetailCommand { get; }

        public MainViewModel()
        {
            // ✅ Opret én fælles AuctionService med baseUrl fra AppState
            _auctionService = new AuctionService(AppState.Instance.ApiBaseUrl);

            // ✅ Start på Home
            CurrentContent = new HomeScreenViewModel(_auctionService);

            // ✅ Init commands
            ShowHomeCommand = new RelayCommand(ShowHomeScreen);
            ShowAuctionOverviewCommand = new RelayCommand(ShowAuctionOverview);
            ShowAuctionDetailCommand = new RelayCommand<AuctionDetailViewModel>(ShowAuctionDetail);
        }
        public MainViewModel(AuctionService auctionService)
        {
            _auctionService = auctionService;
            CurrentContent = new HomeScreenViewModel(_auctionService);

            // Init commands
            ShowHomeCommand = new RelayCommand(ShowHomeScreen);
            ShowAuctionOverviewCommand = new RelayCommand(ShowAuctionOverview);
            ShowAuctionDetailCommand = new RelayCommand<AuctionDetailViewModel>(ShowAuctionDetail);
        }

        private void ShowHomeScreen()
        {
            CurrentContent = new HomeScreenViewModel(_auctionService);
        }

        private void ShowAuctionOverview()
        {
            var vm = new AuctionOverviewViewModel(_auctionService);
            vm.RequestNavigate += Vm_RequestNavigate;
            CurrentContent = vm;
        }

        private void ShowAuctionDetail(AuctionDetailViewModel vm)
        {
            vm.Closed += (_, __) => ShowAuctionOverview();
            CurrentContent = vm;
        }

        private void Vm_RequestNavigate(object? sender, ViewModelBase e)
        {
            if (e is AuctionDetailViewModel detailVm)
                ShowAuctionDetail(detailVm);
            else
                CurrentContent = e;
        }
    }
}
