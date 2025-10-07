using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;


namespace AutoAuction_H2.ViewModels
{
    public class PrivateCarsViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;
        private readonly INavigationService _navigation;

        public ObservableCollection<AuctionEntity> Cars { get; } = new();

        public PrivateCarsViewModel(AuctionService auctionService, INavigationService navigation)
        {
            _auctionService = auctionService;
            _navigation = navigation;
            _ = LoadCarsAsync();
        }

        private async Task LoadCarsAsync()
        {
            try
            {
                var cars = await _auctionService.GetPrivateCarsAsync();

                Cars.Clear();
                foreach (var car in cars)
                    Cars.Add(car);
            }
            catch (System.Exception ex)
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
