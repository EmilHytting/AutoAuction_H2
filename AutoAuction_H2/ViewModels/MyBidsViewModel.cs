using AutoAuction_H2.Models.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels
{
    public class MyBidsViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;
        private readonly INavigationService _navigation;
        public ObservableCollection<AuctionEntity> Bids { get; } = new();

        public MyBidsViewModel(AuctionService auctionService, INavigationService navigation)
        {
            _auctionService = auctionService;
            _navigation = navigation;
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            var userId = AppState.Instance.UserId;
            var auctions = await _auctionService.GetActiveBidsAsync(userId);

            Bids.Clear();
            foreach (var a in auctions
                .OrderByDescending(x => x.HighestBidderId != userId) // true (overbudt) først
                .ThenBy(x => x.EndTime))
            {
                Bids.Add(a);
            }
        }


        public void OpenAuctionDetail(AuctionEntity auction)
        {
            var vm = new AuctionDetailViewModel(
                auction,
                auction.SellerId == AppState.Instance.UserId, // er jeg sælger?
                _auctionService,
                _navigation
            );

            _navigation.NavigateTo(vm);
        }
    }
}
