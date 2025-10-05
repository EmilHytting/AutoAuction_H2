using AutoAuction_H2.Models.Persistence;
using Xunit;

namespace AutoAuction_H2.Tests
{
    public class AuctionClosingTests
    {
        [Fact]
        public void Auction_ShouldNotBeSold_IfNoBids()
        {
            var auction = new AuctionEntity
            {
                MinPrice = 10000,
                CurrentBid = 0,
                IsSold = false
            };

            // Act
            // simulate closing with no bids
            if (auction.CurrentBid >= auction.MinPrice)
                auction.IsSold = true;

            // Assert
            Assert.False(auction.IsSold);
        }

        [Fact]
        public void Auction_ShouldBeSold_WhenCurrentBidMeetsOrExceedsMinPrice()
        {
            var auction = new AuctionEntity
            {
                MinPrice = 10000,
                CurrentBid = 12000,
                IsSold = false
            };

            // Act
            if (auction.CurrentBid >= auction.MinPrice)
                auction.IsSold = true;

            // Assert
            Assert.True(auction.IsSold);
        }

        [Fact]
        public void Auction_ShouldRemainUnsold_WhenCurrentBidBelowMinPrice()
        {
            var auction = new AuctionEntity
            {
                MinPrice = 15000,
                CurrentBid = 12000,
                IsSold = false
            };

            // Act
            if (auction.CurrentBid >= auction.MinPrice)
                auction.IsSold = true;

            // Assert
            Assert.False(auction.IsSold);
        }
    }
}
