using AutoAuction_H2.Models;
using Xunit;

namespace AutoAuction_H2.Tests
{
    public class AuctionFlowTests
    {
        private readonly PrivateUser _seller = new("Seller", "pwd", 8000, 50000, "1111111111");
        private readonly PrivateUser _buyer = new("Buyer", "pwd", 8200, 200000, "2222222222"); // balance hævet

        [Fact]
        public void BidFlow_ShouldAcceptValidBid_AndSell()
        {
            var auction = Auction.CreatePrivateCarAuction(
                _seller, "Audi A3", "XY12345", 2019, 200000, 40000,
                false, 1.8, 16, FuelType.Benzin,
                5, new TrunkDimensions(120, 80, 60), true, 100000);

            Assert.True(auction.PlaceBid(_buyer, 120000));
            Assert.True(auction.AcceptBid(_seller));

            Assert.True(auction.IsSold);
            Assert.Equal(80000, _buyer.Balance); // 200000 - 120000
            Assert.Equal(170000, _seller.Balance); // 50000 + 120000
        }
    }
}
