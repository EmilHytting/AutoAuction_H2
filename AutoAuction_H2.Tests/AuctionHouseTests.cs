using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Models.Interfaces;
using AutoAuction_H2.Services;
using AutoAuction_H2.Tests.Fakes;
using System;
using Xunit;

namespace AutoAuction_H2.Tests
{
    public class AuctionHouseTests
    {
        private readonly IUserService _userService;
        private readonly AuctionHouse _house;
        private readonly PrivateUser _seller;
        private readonly PrivateUser _buyer;

        public AuctionHouseTests()
        {
            _userService = new FakeUserService();
            _house = new AuctionHouse(_userService);

            _seller = _userService.CreatePrivateUser("Seller", "pw", 9000, 100_000m, "1111111111");
            _buyer = _userService.CreatePrivateUser("Buyer", "pw", 9100, 300_000m, "2222222222");
        }

        [Fact]
        public void CanSetAuction_AndFindById()
        {
            var auction = Auction.CreateTruckAuction(
                _seller, "Volvo FH16", "TR12345", 2021, 800_000m, 30_000,
                true, 12, 3, FuelType.Diesel,
                3.8, 15_000, 18, 20_000, 600_000m);

            int id = _house.SetForSale(auction.Vehicle, _seller, 600_000m);
            var found = _house.FindAuctionWithID(id);

            Assert.NotNull(found);
            Assert.Equal("Volvo FH16", found!.Vehicle.Name);
        }

        [Fact]
        public void Auction_ShouldMoveToSold_WhenMinPriceReached_AndTimeExpires()
        {
            var auction = Auction.CreatePrivateCarAuction(
                _seller, "Toyota Yaris", "AA11111", 2022, 180_000m, 10_000,
                false, 1.5, 20, FuelType.Benzin,
                5, 50, true, 100_000m,
                TimeSpan.FromSeconds(60)); // lang varighed

            int id = _house.SetForSale(auction.Vehicle, _seller, 100_000m, TimeSpan.FromSeconds(60));

            // Køber byder over minpris
            var bidAccepted = _house.ReceiveBid(_buyer, id, 150_000m);
            Assert.True(bidAccepted);

            // Tving udløb
            var live = _house.FindAuctionWithID(id);
            live.EndTime = DateTime.UtcNow.AddMilliseconds(-1);
            _house.CheckExpiredAuctions();

            Assert.Single(_house.SoldAuctions);
            Assert.All(_house.SoldAuctions, a => Assert.True(a.IsSold));
        }

        [Fact]
        public void Auction_ShouldMoveToUnsold_WhenNoBids()
        {
            var auction = Auction.CreatePrivateCarAuction(
                _seller, "Ford Fiesta", "BB22222", 2020, 120_000m, 20_000,
                false, 1.2, 18, FuelType.Benzin,
                5, 320, true, 90_000m,
                TimeSpan.FromSeconds(60));

            int id = _house.SetForSale(auction.Vehicle, _seller, 90_000m, TimeSpan.FromSeconds(60));

            // Tving udløb
            var live = _house.FindAuctionWithID(id);
            live.EndTime = DateTime.UtcNow.AddMilliseconds(-1);
            _house.CheckExpiredAuctions();

            Assert.Single(_house.UnsoldAuctions);
            Assert.All(_house.UnsoldAuctions, a => Assert.False(a.IsSold));
        }

        [Fact]
        public void Auction_ShouldMoveToUnsold_WhenBidBelowMinPrice()
        {
            var auction = Auction.CreatePrivateCarAuction(
                _seller, "Mazda 3", "CC33333", 2021, 160_000m, 15_000,
                false, 2.0, 15, FuelType.Benzin,
                5, 320, true, 150_000m,
                TimeSpan.FromSeconds(60));

            int id = _house.SetForSale(auction.Vehicle, _seller, 150_000m, TimeSpan.FromSeconds(60));

            // Bud under minpris, men dækket
            var bidAccepted = _house.ReceiveBid(_buyer, id, 140_000m);
            Assert.True(bidAccepted);

            // Tving udløb
            var live = _house.FindAuctionWithID(id);
            live.EndTime = DateTime.UtcNow.AddMilliseconds(-1);
            _house.CheckExpiredAuctions();

            Assert.Single(_house.UnsoldAuctions);
            Assert.All(_house.UnsoldAuctions, a => Assert.False(a.IsSold));
        }
    }
}
