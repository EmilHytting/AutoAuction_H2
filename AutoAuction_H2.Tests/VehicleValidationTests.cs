using AutoAuction_H2.Models;
using Xunit;

namespace AutoAuction_H2.Tests
{
    public class VehicleValidationTests
    {
        private readonly User _seller = new PrivateUser("Seller", "pw", 8000, 100000, "1111111111");

        [Fact]
        public void RegistrationNumber_ShouldMaskCorrectly()
        {
            var auction = Auction.CreatePrivateCarAuction(
                _seller, "Mazda 3", "AB12345", 2020, 180000, 30000,
                false, 2.0, 16, FuelType.Benzin, 5,
                new TrunkDimensions(120, 80, 50), true, 120000);

            Assert.Equal("**123**", auction.Vehicle.RegistrationNumber);
        }

        [Fact]
        public void NegativeMileage_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                Auction.CreatePrivateCarAuction(
                    _seller, "Mazda 3", "AB12345", 2020, 180000, -30000,
                    false, 2.0, 16, FuelType.Benzin, 5,
                    new TrunkDimensions(120, 80, 50), true, 120000));
        }
    }
}
