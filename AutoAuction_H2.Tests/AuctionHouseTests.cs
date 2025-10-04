using AutoAuction_H2.Models;
using AutoAuction_H2.Services;
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
            _userService = new UserService();
            _house = new AuctionHouse(_userService);

            // Brugere SKAL oprettes via UserService
            _seller = _userService.CreatePrivateUser("Seller", "pw", 9000, 100000, "1111111111");
            _buyer = _userService.CreatePrivateUser("Buyer", "pw", 9100, 300000, "2222222222");
        }

        [Fact]
        public void CanSetAuction_AndFindById()
        {
            var auction = Auction.CreateTruckAuction(
                _seller, "Volvo FH16", "TR12345", 2021, 800000, 30000,
                true, 12, 3, FuelType.Diesel, 3.8, 15000, 18, 20000,
                600000);

            int id = _house.SetForSale(auction.Vehicle, _seller, 600000);
            var found = _house.FindAuctionWithID(id);

            Assert.NotNull(found);
            Assert.Equal("Volvo FH16", found!.Vehicle.Name);
        }

        [Fact]
        public void AcceptBid_ShouldMoveAuctionToSoldList()
        {
            var auction = Auction.CreatePrivateCarAuction(
                _seller, "Toyota Yaris", "AA11111", 2022, 180000, 10000,
                false, 1.5, 20, FuelType.Benzin,
                5, new TrunkDimensions(90, 70, 50), true, 100000);

            int id = _house.SetForSale(auction.Vehicle, _seller, 100000);
            _house.ReceiveBid(_buyer, id, 150000);
            _house.AcceptBid(_seller, id);

            Assert.Single(_house.SoldAuctions);
            Assert.All(_house.SoldAuctions, a => Assert.True(a.IsSold));
        }
    }
}
