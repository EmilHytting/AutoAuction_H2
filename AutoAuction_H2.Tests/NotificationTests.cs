using AutoAuction_H2.Models.Entities;
using Xunit;

namespace AutoAuction_H2.Tests
{
    public class NotificationTests
    {
        private class NotifyingSeller : PrivateUser
        {
            public bool Notified { get; private set; }

            public NotifyingSeller(string username, string password, int zip, decimal balance, string cpr)
                : base(username, password, zip, balance, cpr) { }

            public override void NotifyAboutBid(Auction auction, decimal amount)
            {
                Notified = true;
            }
        }

        [Fact]
        public void Seller_IsNotified_WhenBidIsAboveMinPrice()
        {
            var seller = new NotifyingSeller("Seller", "pw", 8000, 50000, "1111111111");
            var buyer = new PrivateUser("Buyer", "pw", 8200, 200000, "2222222222");

            var auction = Auction.CreatePrivateCarAuction(
                seller, "Audi A4", "AB12345", 2020, 250000, 30000,
                false, 2.0, 18, FuelType.Benzin, 5,
                320, true, 100000);

            auction.PlaceBid(buyer, 120000);

            Assert.True(seller.Notified);
        }

        [Fact]
        public void Seller_IsNotNotified_WhenBidIsBelowMinPrice()
        {
            var seller = new NotifyingSeller("Seller", "pw", 8000, 50000, "1111111111");
            var buyer = new PrivateUser("Buyer", "pw", 8200, 200000, "2222222222");

            var auction = Auction.CreatePrivateCarAuction(
                seller, "Audi A4", "AB12345", 2020, 250000, 30000,
                false, 2.0, 18, FuelType.Benzin, 5,
                320, true, 100000);

            auction.PlaceBid(buyer, 50000);

            Assert.False(seller.Notified);
        }
    }
}
