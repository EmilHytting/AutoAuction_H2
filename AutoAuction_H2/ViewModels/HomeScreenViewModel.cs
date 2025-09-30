using System.Collections.ObjectModel;

namespace AutoAuction_H2.ViewModels
{
    public class AuctionMock
    {
        public int AuctionId { get; set; }
        public string VehicleName { get; set; } = string.Empty;
        public double CurrentBid { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class HomeScreenViewModel
    {
        public ObservableCollection<AuctionMock> MockAuctions { get; } = new()
        {
            new AuctionMock { AuctionId = 1, VehicleName = "VW Golf", CurrentBid = 120000, Status = "Active" },
            new AuctionMock { AuctionId = 2, VehicleName = "Volvo Truck", CurrentBid = 350000, Status = "Ended" },
            new AuctionMock { AuctionId = 3, VehicleName = "Ford Fiesta", CurrentBid = 90000, Status = "Active" }
        };
    }
}