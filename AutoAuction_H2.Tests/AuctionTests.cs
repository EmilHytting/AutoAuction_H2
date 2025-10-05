using System;
using AutoAuction_H2.Models.Entities;
using Xunit;

namespace AutoAuction_H2.Tests
{
    public class AuctionTests
    {
        [Fact]
        public void PlaceBid_ShouldUpdateHighestBidder_WhenBidIsHigher()
        {
            var seller = new PrivateUser("Seller", "pw", 9000, 0, "1234567890");
            var buyer = new PrivateUser("Buyer", "pw", 8000, 50000, "0987654321");

            var car = new PrivateCar("VW Golf", "AB12345", 2018, 150000, 60000,
                false, 1.6, 18.5, FuelType.Benzin, 5, 300, true);
            var auction = new Auction(car, seller, 20000);

            auction.PlaceBid(buyer, 25000);

            Assert.Equal(25000, auction.CurrentBid);
            Assert.Equal(buyer, auction.HighestBidder);
        }

        [Fact]
        public void PlaceBid_ShouldNotAcceptLowerBid()
        {
            var seller = new PrivateUser("Seller", "pw", 9000, 0, "1234567890");
            var buyer = new PrivateUser("Buyer", "pw", 8000, 50000, "0987654321");

            var car = new PrivateCar("VW Golf", "AB12345", 2018, 150000, 60000,
                false, 1.6, 18.5, FuelType.Benzin, 5, 300, true);
            var auction = new Auction(car, seller, 20000);

            auction.PlaceBid(buyer, 25000);
            auction.PlaceBid(buyer, 24000); // lavere bud

            Assert.Equal(25000, auction.CurrentBid); // skal forblive uændret
        }

        [Fact]
        public void Close_ShouldTransferMoney_WhenAboveMinPrice_AndBuyerCanPay()
        {
            var seller = new PrivateUser("Seller", "pw", 9000, 1000, "1234567890");
            var buyer = new CorporateUser("Firm", "pw", 9000, 5000, "12345678", 25000); // kredit inkluderet

            var car = new PrivateCar("VW Golf", "AB12345", 2018, 150000, 60000,
                false, 1.6, 18.5, FuelType.Benzin, 5, 300, true);
            var auction = new Auction(car, seller, 20000);

            auction.PlaceBid(buyer, 25000);
            bool closed = auction.Close();

            Assert.True(closed);
            Assert.True(auction.IsSold);
            Assert.Equal(26000, seller.Balance);   // 1000 + 25000
            Assert.Equal(-20000, buyer.Balance);   // 5000 - 25000
        }

        [Fact]
        public void Close_ShouldFail_WhenBidBelowMinPrice()
        {
            var seller = new PrivateUser("Seller", "pw", 9000, 1000, "1234567890");
            var buyer = new PrivateUser("Buyer", "pw", 8000, 50000, "0987654321");

            var car = new PrivateCar("VW Golf", "AB12345", 2018, 150000, 60000,
                false, 1.6, 18.5, FuelType.Benzin, 5, 300, true);
            var auction = new Auction(car, seller, 20000);

            auction.PlaceBid(buyer, 15000); // under minpris
            bool closed = auction.Close();

            Assert.False(closed);
            Assert.False(auction.IsSold);
            Assert.Equal(1000, seller.Balance);   // uændret
            Assert.Equal(50000, buyer.Balance);   // uændret
        }

        [Fact]
        public void Close_ShouldFail_WhenBuyerCannotCoverBid()
        {
            var seller = new PrivateUser("Seller", "pw", 9000, 1000, "1234567890");
            var buyer = new CorporateUser("Firm", "pw", 9000, 1000, "12345678", 2000); // 1000 + 2000 kredit = 3000

            var car = new PrivateCar("VW Golf", "AB12345", 2018, 150000, 60000,
                false, 1.6, 18.5, FuelType.Benzin, 5, 300, true);
            var auction = new Auction(car, seller, 20000);

            auction.PlaceBid(buyer, 25000); // langt over hvad buyer kan betale
            bool closed = auction.Close();

            Assert.False(closed);
            Assert.False(auction.IsSold);
            Assert.Equal(1000, seller.Balance);   // uændret
            Assert.Equal(1000, buyer.Balance);    // uændret
        }

        [Fact]
        public void PlaceBid_ShouldReserveBuyerFunds()
        {
            var seller = new PrivateUser("Seller", "pw", 9000, 0, "1234567890");
            var buyer = new PrivateUser("Buyer", "pw", 8000, 30000, "0987654321");

            var car = new PrivateCar("VW Golf", "AB12345", 2018, 150000, 60000,
                false, 1.6, 18.5, FuelType.Benzin, 5, 300, true);

            var auction = new Auction(car, seller, 20000);

            // Act
            bool placed = auction.PlaceBid(buyer, 25000);

            // Assert
            Assert.True(placed);
            Assert.Equal(25000, buyer.ReservedAmount); // pengene er reserveret
            Assert.Equal(5000, buyer.AvailableBalance); // tilbageværende
        }

        [Fact]
        public void PlaceBid_ShouldFail_IfNotEnoughAvailableBalance()
        {
            var seller = new PrivateUser("Seller", "pw", 9000, 0, "1234567890");
            var buyer = new PrivateUser("Buyer", "pw", 8000, 20000, "0987654321");

            var car = new PrivateCar("VW Golf", "AB12345", 2018, 150000, 60000,
                false, 1.6, 18.5, FuelType.Benzin, 5, 300, true);

            var auction = new Auction(car, seller, 20000);

            // Act
            bool placed = auction.PlaceBid(buyer, 25000);

            // Assert
            Assert.False(placed);
            Assert.Equal(0, buyer.ReservedAmount); // ingen reservation sket
        }

    }
}
