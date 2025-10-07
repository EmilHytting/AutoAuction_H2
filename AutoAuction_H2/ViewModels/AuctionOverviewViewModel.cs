using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels
{
    public class AuctionOverviewViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;
        private readonly INavigationService _navigation; // ✅ For navigating to detail view

        public ObservableCollection<AuctionEntity> ActiveAuctions { get; } = new();

        public AuctionOverviewViewModel(AuctionService auctionService, INavigationService navigation)
        {
            _auctionService = auctionService;
            _navigation = navigation;
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                var auctions = await _auctionService.GetAuctionsAsync();

                ActiveAuctions.Clear();
                foreach (var auction in auctions)
                    ActiveAuctions.Add(auction);
            }
            catch (System.Exception)
            {
                // TODO: evt. fejlbesked property til UI
            }
        }

        // ✅ Called from code-behind when row is clicked/right-clicked
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
