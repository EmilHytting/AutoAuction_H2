using System;
using AutoAuction_H2.Models.Entities;
using Xunit;

namespace AutoAuction_H2.Tests
{
    public class AuctionTimeTests
    {
        [Fact]
        public void PlaceBid_ShouldFail_WhenAuctionExpired()
        {
            // Arrange
            var seller = new PrivateUser("Seller", "pw", 9000, 0, "1234567890");
            var buyer = new PrivateUser("Buyer", "pw", 8000, 50000, "0987654321");
            var car = new PrivateCar("VW Golf", "AB12345", 2018, 150000, 60000,
                false, 1.6, 18.5, FuelType.Benzin, 5, 300, true);

            // Sæt auction til at være udløbet
            var auction = new Auction(car, seller, 20000, TimeSpan.FromSeconds(1));
            auction.EndTime = DateTime.UtcNow.AddSeconds(-1);

            // Act
            bool result = auction.PlaceBid(buyer, 25000);

            // Assert
            Assert.False(result);
            Assert.Null(auction.HighestBidder);
            Assert.Equal(0, auction.CurrentBid);
        }

        [Fact]
        public void PlaceBid_ShouldExtendAuction_WhenCloseToEnd()
        {
            // Arrange
            var seller = new PrivateUser("Seller", "pw", 9000, 0, "1234567890");
            var buyer = new PrivateUser("Buyer", "pw", 8000, 50000, "0987654321");
            var car = new PrivateCar("VW Golf", "AB12345", 2018, 150000, 60000,
                false, 1.6, 18.5, FuelType.Benzin, 5, 300, true);

            var auction = new Auction(car, seller, 20000, TimeSpan.FromSeconds(5));
            auction.EndTime = DateTime.UtcNow.AddSeconds(10); // tæt på slutningen

            var beforeBidEndTime = auction.EndTime;

            // Act
            bool result = auction.PlaceBid(buyer, 25000);

            // Assert
            Assert.True(result);
            Assert.NotNull(auction.HighestBidder);
            Assert.True(auction.EndTime > beforeBidEndTime); // tid blev forlænget
        }
    }
}
