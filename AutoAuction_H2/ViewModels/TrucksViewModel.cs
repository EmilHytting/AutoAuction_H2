using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Services;
using System;
using AutoAuction_H2.Models.Persistence;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
namespace AutoAuction_H2.ViewModels
{
    public class TrucksViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;
        private readonly INavigationService _navigation;

        public ObservableCollection<AuctionEntity> Trucks { get; } = new();

        public TrucksViewModel(AuctionService auctionService, INavigationService navigation)
        {
            _auctionService = auctionService;
            _navigation = navigation;
            _ = LoadTrucksAsync();
        }

        private async Task LoadTrucksAsync()
        {
            try
            {
                var auctions = await _auctionService.GetTrucksAsync();

                Trucks.Clear();
                foreach (var a in auctions.Where(a => a.Vehicle is TruckEntity))
                    Trucks.Add(a);
            }
            catch (System.Exception)
            {
                // TODO: fejlbesked property til UI
            }
        }
        public void OpenAuctionDetail(AuctionEntity auction)
        {
            var vm = new AuctionDetailViewModel(
                auction,
                auction.SellerId == AppState.Instance.UserId,
                _auctionService,
                _navigation   // 👈 navigation skal med
            );
            _navigation.NavigateTo(vm);
        }
    }
}
