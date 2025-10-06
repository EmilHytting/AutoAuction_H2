using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xunit.Sdk;

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

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string? errorMessage;

        public HomeScreenViewModel(AuctionService auctionService)
        {
            _auctionService = auctionService;
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var userId = AppState.Instance.UserId;

                // Hent mine auktioner
                var my = await _auctionService.GetMyAuctionsAsync(userId);
                MyAuctions = new ObservableCollection<AuctionEntity>(my);

                // Hent aktive bud
                var bids = await _auctionService.GetActiveBidsAsync(userId);
                ActiveBids = new ObservableCollection<AuctionEntity>(bids);

                // Hent auktioner hvor jeg er overbudt
                var overbid = await _auctionService.GetOverbidAuctionsAsync(userId);
                OverbidAuctions = new ObservableCollection<AuctionEntity>(overbid);

                // Hvis du vil hente alle auktioner bare til debug/test
                // var all = await _auctionService.GetAuctionsAsync();
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Kunne ikke loade data: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
