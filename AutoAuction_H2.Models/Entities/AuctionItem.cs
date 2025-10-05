using System;

namespace AutoAuction_H2.Models.Entities;

public class AuctionItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal CurrentBid { get; set; }
    public DateTime ClosingDate { get; set; }
}
