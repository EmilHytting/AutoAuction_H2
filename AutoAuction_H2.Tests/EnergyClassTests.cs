using AutoAuction_H2.Models.Entities;
using Xunit;

namespace AutoAuction_H2.Tests
{
    public class EnergyClassTests
    {
        private readonly PrivateUser _seller = new("Seller", "pw", 8000, 50000, "1111111111");

        [Theory]
        [InlineData(FuelType.Diesel, 2015, 26, EnergyClass.A)]
        [InlineData(FuelType.Diesel, 2008, 19, EnergyClass.B)]
        [InlineData(FuelType.Benzin, 2012, 13, EnergyClass.C)]
        [InlineData(FuelType.Benzin, 2009, 9, EnergyClass.D)]
        public void EnergyClass_CalculatesCorrectly(FuelType fuel, int year, double kmPerL, EnergyClass expected)
        {
            var auction = Auction.CreatePrivateCarAuction(
                _seller, "TestCar", "ZZ99999", year, 100000, 50000, false,
                1.6, kmPerL, fuel, 5,
                320, true, 50000);

            Assert.Equal(expected, auction.Vehicle.GetEnergyClass());
        }
    }
}
