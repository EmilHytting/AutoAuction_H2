namespace AutoAuction_H2.Models;

public class AuctionItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal CurrentBid { get; set; }
    public System.DateTime ClosingDate { get; set; }
}
