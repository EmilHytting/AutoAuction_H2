using AutoAuction_H2.Models;
using AutoAuction_H2.Services;
using Xunit;

namespace AutoAuction_H2.Tests
{
    public class AuctionIntegrationTests
    {
        private readonly IUserService _userService;
        private readonly AuctionHouse _auctionHouse;

        public AuctionIntegrationTests()
        {
            _userService = new UserService();
            _auctionHouse = new AuctionHouse(_userService);
        }

        [Fact]
        public void FullAuctionFlow_ShouldWork()
        {
            // ---------- Arrange ----------
            // Opret sælger og køber
            var seller = _userService.CreatePrivateUser("Seller", "pw", 8000, 1000m, "1234567890");
            var buyer = _userService.CreateCorporateUser("BuyerFirm", "pw", 9000, 5000m, "87654321", 2000m);

            // Opret køretøj
            var car = new PrivateCar("VW Golf", "AB12345", 2018, 150000m, 80000, true, 1.6, 18, FuelType.Benzin, 5, new TrunkDimensions(100, 50, 70), true);

            // ---------- Act ----------
            // Sæt bilen til salg
            int auctionId = _auctionHouse.SetForSale(car, seller, 100000m);

            // Køber afgiver bud
            bool bidAccepted = _auctionHouse.ReceiveBid(buyer, auctionId, 110000m);

            // Sælger accepterer bud
            bool saleCompleted = _auctionHouse.AcceptBid(seller, auctionId);

            // ---------- Assert ----------
            Assert.True(bidAccepted);
            Assert.True(saleCompleted);

            // Auktionen skal være flyttet til solgte
            var soldAuction = _auctionHouse.SoldAuctions.FirstOrDefault(a => a.Id == auctionId);
            Assert.NotNull(soldAuction);
            Assert.True(soldAuction.IsSold);

            // Balancer skal være opdateret
            Assert.Equal(1000m + 110000m, seller.Balance); // sælger fik pengene
            Assert.Equal(5000m - 110000m, buyer.Balance);  // køber betalte
        }
    }
}
