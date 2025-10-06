using AutoAuction_H2.Models.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace AutoAuction_H2.ViewModels
{
    public class ProfessionalCarsViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;
        public ObservableCollection<AuctionEntity> Cars { get; } = new();

        public ProfessionalCarsViewModel(AuctionService auctionService)
        {
            _auctionService = auctionService;
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            var auctions = await _auctionService.GetAuctionsAsync();
            Cars.Clear();
            foreach (var a in auctions.Where(a => a.Vehicle is ProfessionalCar))
                Cars.Add(a);
        }
    }
}