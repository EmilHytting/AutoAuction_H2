using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuction_H2.Models
{
    public class Auction
    {
        public int Id { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public User Seller { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal StartPrice { get; set; }
        public decimal CurrentBid { get; set; }
        public User? HighestBidder { get; set; }
        public bool IsClosed => DateTime.Now >= EndTime;
    }

}
