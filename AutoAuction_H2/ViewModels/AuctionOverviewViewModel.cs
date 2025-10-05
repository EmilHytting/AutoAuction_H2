using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels
{
    public partial class AuctionOverviewViewModel : ViewModelBase
    {
        private readonly AuctionService? _auctionService;

        [ObservableProperty]
        private ObservableCollection<AuctionEntity> allAuctions = new();

        public IRelayCommand<AuctionEntity> OpenBuyerDetailCommand { get; }
        public IRelayCommand<AuctionEntity> OpenSellerDetailCommand { get; }

        // ✅ Bruges af MainViewModel
        public AuctionOverviewViewModel(AuctionService auctionService)
        {
            _auctionService = auctionService;
            OpenBuyerDetailCommand = new RelayCommand<AuctionEntity>(OpenBuyer);
            OpenSellerDetailCommand = new RelayCommand<AuctionEntity>(OpenSeller);
            _ = LoadDataAsync();
        }

        // ✅ Behold parameterløs constructor til design preview eller tests
        public AuctionOverviewViewModel()
        {
            OpenBuyerDetailCommand = new RelayCommand<AuctionEntity>(OpenBuyer);
            OpenSellerDetailCommand = new RelayCommand<AuctionEntity>(OpenSeller);
        }

        private async Task LoadDataAsync()
        {
            if (_auctionService is null)
                return;

            var auctions = await _auctionService.GetAuctionsAsync();
            AllAuctions = new ObservableCollection<AuctionEntity>(auctions);
        }

        public event EventHandler<ViewModelBase>? RequestNavigate;

        private void OpenBuyer(AuctionEntity? auction)
        {
            if (auction is null) return;
            var vm = new AutoAuction_H2.ViewModels.Auction.AuctionDetailViewModel(auction, isSeller: false);
            RequestNavigate?.Invoke(this, vm);
        }

        private void OpenSeller(AuctionEntity? auction)
        {
            if (auction is null) return;
            var vm = new AutoAuction_H2.ViewModels.Auction.AuctionDetailViewModel(auction, isSeller: true);
            RequestNavigate?.Invoke(this, vm);
        }
    }
}
