using AutoAuction_H2.Models.Persistence;
using Xunit;

namespace AutoAuction_H2.Tests
{
    public class AuctionEntityTests
    {
        [Fact]
        public void BidLowerThanMinPrice_ShouldNotBeAccepted()
        {
            // Arrange
            var auction = new AuctionEntity
            {
                MinPrice = 10000,
                CurrentBid = 0
            };

            var bid = new BidEntity
            {
                Amount = 5000
            };

            // Act & Assert
            Assert.True(bid.Amount < auction.MinPrice);
        }

        [Fact]
        public void HigherBid_ShouldUpdateCurrentBid()
        {
            // Arrange
            var auction = new AuctionEntity
            {
                MinPrice = 10000,
                CurrentBid = 10000
            };

            var bid = new BidEntity
            {
                Amount = 12000
            };

            // Act
            auction.CurrentBid = bid.Amount;

            // Assert
            Assert.Equal(12000, auction.CurrentBid);
        }
    }
}
