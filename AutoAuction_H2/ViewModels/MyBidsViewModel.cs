using AutoAuction_H2.Models.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels
{
    public class MyBidsViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;
        public ObservableCollection<AuctionEntity> Bids { get; } = new();

        public MyBidsViewModel(AuctionService auctionService)
        {
            _auctionService = auctionService;
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            var userId = AppState.Instance.UserId;
            var auctions = await _auctionService.GetActiveBidsAsync(userId);
            Bids.Clear();
            foreach (var a in auctions)
                Bids.Add(a);
        }
    }

}
