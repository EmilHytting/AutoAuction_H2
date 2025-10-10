using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Models.Persistence;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class AuctionEntity
{
    public int Id { get; set; }

    public int VehicleId { get; set; }
    public VehicleEntity Vehicle { get; set; } = null!;

    public int SellerId { get; set; }
    public UserEntity Seller { get; set; } = null!;

    public int? HighestBidderId { get; set; }
    public UserEntity? HighestBidder { get; set; }
    public bool IsOverbid => HighestBidderId != AppState.Instance.UserId;
    public decimal MinPrice { get; set; }
    public decimal CurrentBid { get; set; }
    public bool IsSold { get; set; }
    public DateTime EndTime { get; set; }

    [JsonIgnore]  // 🚀 prevents serialization loop
    public List<BidEntity> Bids { get; set; } = new();
}
