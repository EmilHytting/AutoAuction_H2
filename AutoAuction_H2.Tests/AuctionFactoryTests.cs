using AutoAuction_H2.Models.Entities;
using Xunit;

namespace AutoAuction_H2.Tests
{
    public class AuctionFactoryTests
    {
        private readonly User _seller = new PrivateUser("TestSeller", "1234", 8000, 10000, "1111111111");

        [Fact]
        public void CreatePrivateCarAuction_ShouldReturnAuctionWithPrivateCar()
        {
            var auction = Auction.CreatePrivateCarAuction(
                _seller, "VW Polo", "AB12345", 2020, 120000, 25000, false,
                1.2, 18, FuelType.Benzin, 5,
                320, true, 100000);

            Assert.IsType<PrivateCar>(auction.Vehicle);
            Assert.Equal("VW Polo", auction.Vehicle.Name);
        }

        [Fact]
        public void CreateBusAuction_ShouldReturnAuctionWithBus()
        {
            var auction = Auction.CreateBusAuction(
                _seller, "Scania Touring", "BC54321", 2018, 1500000, 40000, true,
                12, 3.0, FuelType.Diesel, 3.5, 12000, 15,
                50, 10, true, 1000000);

            Assert.IsType<Bus>(auction.Vehicle);
            Assert.Equal(50, ((Bus)auction.Vehicle).PassengerSeats);
        }
    }
}
