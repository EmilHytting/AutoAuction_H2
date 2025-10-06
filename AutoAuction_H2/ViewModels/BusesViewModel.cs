using AutoAuction_H2.Models.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels
{
    public class BusesViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;
        public ObservableCollection<AuctionEntity> Buses { get; } = new();

        public BusesViewModel(AuctionService auctionService)
        {
            _auctionService = auctionService;
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            var auctions = await _auctionService.GetAuctionsAsync();
            Buses.Clear();
            foreach (var a in auctions.Where(a => a.Vehicle is Bus))
                Buses.Add(a);
        }
    }

}
