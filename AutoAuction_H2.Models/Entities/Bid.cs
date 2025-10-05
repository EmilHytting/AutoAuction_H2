using System;
using AutoAuction_H2.Models.Interfaces;
namespace AutoAuction_H2.Models.Entities
{
    public class Bid
    {
        public int Id { get; private set; }
        public IUser Bidder { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Timestamp { get; private set; }

        private Bid() { } // EF Core

        public Bid(IUser bidder, decimal amount)
        {
            if (bidder == null) throw new ArgumentNullException(nameof(bidder));
            if (amount <= 0) throw new ArgumentException("Budbeløbet skal være større end 0.");

            Bidder = bidder;
            Amount = amount;
            Timestamp = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"{Bidder.UserName} bød {Amount} kr. ({Timestamp})";
        }
    }
}
