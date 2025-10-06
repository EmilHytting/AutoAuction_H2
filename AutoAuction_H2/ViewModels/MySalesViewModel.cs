using AutoAuction_H2.Models.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels
{
    public class MySalesViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;
        public ObservableCollection<AuctionEntity> Sales { get; } = new();

        public MySalesViewModel(AuctionService auctionService)
        {
            _auctionService = auctionService;
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            var userId = AppState.Instance.UserId;
            var auctions = await _auctionService.GetMyAuctionsAsync(userId);
            Sales.Clear();
            foreach (var a in auctions)
                Sales.Add(a);
        }
    }
}
