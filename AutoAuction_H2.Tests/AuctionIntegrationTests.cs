using AutoAuction_H2.Services;
using Xunit;
using System;
using System.Linq;
using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Models.Interfaces;

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
        public void FullAuctionFlow_ShouldSell_WhenMinPriceReached()
        {
            // ---------- Arrange ----------
            var seller = _userService.CreatePrivateUser("Seller", "pw", 8000, 5_000m, "1234567890");
            var buyer = _userService.CreateCorporateUser("BuyerFirm", "pw", 9000, 200_000m, "87654321", 50_000m);

            var car = new PrivateCar("VW Golf", "AB12345", 2018, 150_000m, 80_000, true,
                                     1.6, 18, FuelType.Benzin, 5,
                                     320, true);

            // ---------- Act ----------
            int auctionId = _auctionHouse.SetForSale(car, seller, 100_000m, TimeSpan.FromSeconds(60)); // lang varighed så poacher-safe ikke trigges utilsigtet
            bool bidAccepted = _auctionHouse.ReceiveBid(buyer, auctionId, 120_000m);
            Assert.True(bidAccepted);

            // Tving udløb (omgå poacher-safe i test)
            var auction = _auctionHouse.FindAuctionWithID(auctionId);
            auction.EndTime = DateTime.UtcNow.AddMilliseconds(-1);
            _auctionHouse.CheckExpiredAuctions();

            // ---------- Assert ----------
            var soldAuction = _auctionHouse.SoldAuctions.FirstOrDefault(a => a.Id == auctionId);
            Assert.NotNull(soldAuction);
            Assert.True(soldAuction!.IsSold);

            Assert.Equal(5_000m + 120_000m, seller.Balance);   // sælger fik pengene
            Assert.Equal(200_000m - 120_000m, buyer.Balance);   // køber betalte
        }

        [Fact]
        public void FullAuctionFlow_ShouldNotSell_WhenNoBids()
        {
            var seller = _userService.CreatePrivateUser("Seller2", "pw", 8100, 1_000m, "2345678901");

            var car = new PrivateCar("Ford Fiesta", "CD67890", 2020, 120_000m, 20_000, false,
                                     1.2, 18, FuelType.Benzin, 5,
                                     320, true);

            int auctionId = _auctionHouse.SetForSale(car, seller, 90_000m, TimeSpan.FromSeconds(60));

            // Tving udløb
            var auction = _auctionHouse.FindAuctionWithID(auctionId);
            auction.EndTime = DateTime.UtcNow.AddMilliseconds(-1);
            _auctionHouse.CheckExpiredAuctions();

            var unsoldAuction = _auctionHouse.UnsoldAuctions.FirstOrDefault(a => a.Id == auctionId);
            Assert.NotNull(unsoldAuction);
            Assert.False(unsoldAuction!.IsSold);
        }

        [Fact]
        public void FullAuctionFlow_ShouldNotSell_WhenBidBelowMinPrice()
        {
            var seller = _userService.CreatePrivateUser("Seller3", "pw", 8200, 2_000m, "3456789012");
            // Køber med 50k saldo – byder 40k (under minpris 150k), men har dækning ⇒ bud accepteres
            var buyer = _userService.CreatePrivateUser("Buyer3", "pw", 8300, 50_000m, "4567890123");

            var car = new PrivateCar("Mazda 3", "EF98765", 2021, 160_000m, 15_000, false,
                                     2.0, 15, FuelType.Benzin, 5,
                                     320, true);

            int auctionId = _auctionHouse.SetForSale(car, seller, 150_000m, TimeSpan.FromSeconds(60));

            // Bud UNDER minpris men DÆKKET af købers saldo ⇒ bud skal accepteres
            bool bidAccepted = _auctionHouse.ReceiveBid(buyer, auctionId, 40_000m);
            Assert.True(bidAccepted);

            // Tving udløb
            var auction = _auctionHouse.FindAuctionWithID(auctionId);
            auction.EndTime = DateTime.UtcNow.AddMilliseconds(-1);
            _auctionHouse.CheckExpiredAuctions();

            var unsoldAuction = _auctionHouse.UnsoldAuctions.FirstOrDefault(a => a.Id == auctionId);
            Assert.NotNull(unsoldAuction);
            Assert.False(unsoldAuction!.IsSold);

            // Saldo ændres ikke fordi minpris ikke blev mødt
            Assert.Equal(50_000m, buyer.Balance);
            Assert.Equal(2_000m, seller.Balance);
        }
    }
}
