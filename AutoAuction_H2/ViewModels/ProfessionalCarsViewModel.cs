using AutoAuction_H2.Models.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace AutoAuction_H2.ViewModels
{
    public class ProfessionalCarsViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;
        private readonly INavigationService _navigation;
        public ObservableCollection<AuctionEntity> Cars { get; } = new();

        public ProfessionalCarsViewModel(AuctionService auctionService, INavigationService navigationService)
        {
            _auctionService = auctionService;
            _navigation = navigationService;
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            var auctions = await _auctionService.GetProfessionalCarsAsync();
            Cars.Clear();
            foreach (var car in auctions)
                Cars.Add(car);
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