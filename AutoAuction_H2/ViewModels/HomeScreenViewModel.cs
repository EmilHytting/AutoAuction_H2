using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;


namespace AutoAuction_H2.ViewModels
{
    public partial class HomeScreenViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;

        [ObservableProperty]
        private ObservableCollection<AuctionEntity> myAuctions = new();

        [ObservableProperty]
        private ObservableCollection<AuctionEntity> activeBids = new();

        [ObservableProperty]
        private ObservableCollection<AuctionEntity> overbidAuctions = new();

        public HomeScreenViewModel(AuctionService auctionService)
        {
            _auctionService = auctionService;
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var userId = AppState.Instance.UserId;

            // Hent mine auktioner
            MyAuctions = new ObservableCollection<AuctionEntity>(
                await _auctionService.GetMyAuctionsAsync(userId));

            // Hent aktive bud
            ActiveBids = new ObservableCollection<AuctionEntity>(
                await _auctionService.GetActiveBidsAsync(userId));

            // Hent auktioner hvor jeg er overbudt
            OverbidAuctions = new ObservableCollection<AuctionEntity>(
                await _auctionService.GetOverbidAuctionsAsync(userId));

            // Hvis du vil have "alle auktioner":
            var all = await _auctionService.GetAuctionsAsync();
        }


    }
}
