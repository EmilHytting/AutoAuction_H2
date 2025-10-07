using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Models.Persistence;
using AutoAuction_H2.Services;   // ✅ Husk denne
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels
{
    public class BusesViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;
        private readonly INavigationService _navigation;
        public ObservableCollection<AuctionEntity> Buses { get; } = new();

        public BusesViewModel(AuctionService auctionService, INavigationService navigation)
        {
            _auctionService = auctionService;
            _navigation = navigation;
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            var auctions = await _auctionService.GetBusesAsync();

            Buses.Clear();
            foreach (var a in auctions.Where(a => a.Vehicle is BusEntity))
                Buses.Add(a);
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
