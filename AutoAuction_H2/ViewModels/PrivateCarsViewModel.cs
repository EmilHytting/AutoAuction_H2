using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;


namespace AutoAuction_H2.ViewModels
{
    public class PrivateCarsViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;

        public ObservableCollection<AuctionEntity> Cars { get; } = new();

        public PrivateCarsViewModel(AuctionService auctionService)
        {
            _auctionService = auctionService;
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
    }
}
