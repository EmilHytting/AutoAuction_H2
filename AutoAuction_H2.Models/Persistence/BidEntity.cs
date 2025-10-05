using System;
using System.Text.Json.Serialization;
namespace AutoAuction_H2.Models.Persistence
{
    public class BidEntity
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }

        [JsonIgnore] // prevent cycles
        public AuctionEntity Auction { get; set; } = null!;

        public UserEntity User { get; set; } = null!;
    }
}
