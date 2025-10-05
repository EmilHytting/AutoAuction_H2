using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoAuction_H2.Models.Interfaces;

namespace AutoAuction_H2.Models.Entities
{
    public class AuctionHouse
    {
        private readonly ConcurrentDictionary<int, Auction> _auctions = new();
        private readonly List<Auction> _soldAuctions = new();
        private readonly List<Auction> _unsoldAuctions = new();
        private int _auctionCounter = 0;
        private readonly object _lock = new();
        private readonly IUserService _userService;

        public IEnumerable<Auction> ActiveAuctions => _auctions.Values;
        public IEnumerable<Auction> SoldAuctions => _soldAuctions;
        public IEnumerable<Auction> UnsoldAuctions => _unsoldAuctions;

        public AuctionHouse(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        // ---------- Opret auktion ----------
        public int SetForSale(Vehicle vehicle, IUser seller, decimal minPrice, TimeSpan? duration = null)
        {
            if (_userService.FindByUserName(seller.UserName) == null)
                throw new InvalidOperationException("Sælgeren er ikke registreret i systemet.");

            var auction = new Auction(vehicle, seller, minPrice, duration);
            int auctionId = Interlocked.Increment(ref _auctionCounter);

            auction.GetType().GetProperty("Id")?.SetValue(auction, auctionId);

            if (!_auctions.TryAdd(auctionId, auction))
                throw new InvalidOperationException("Kunne ikke tilføje auktion.");

            return auctionId;
        }

        // ---------- Bud ----------
        public bool ReceiveBid(IUser buyer, int auctionId, decimal bid)
        {
            if (_userService.FindByUserName(buyer.UserName) == null)
                throw new InvalidOperationException("Køberen er ikke registreret i systemet.");

            if (!_auctions.TryGetValue(auctionId, out var auction))
                throw new KeyNotFoundException($"Auktion med ID {auctionId} blev ikke fundet.");

            return auction.PlaceBid(buyer, bid);
        }

        // ---------- Luk manuelt ----------
        public bool CloseAuction(int auctionId)
        {
            if (!_auctions.TryRemove(auctionId, out var auction))
                throw new KeyNotFoundException($"Auktion med ID {auctionId} blev ikke fundet.");

            bool success = auction.Close();

            lock (_lock)
            {
                if (success)
                    _soldAuctions.Add(auction);
                else
                    _unsoldAuctions.Add(auction);
            }

            return success;
        }

        // ---------- Tjek for udløbne auktioner ----------
        public void CheckExpiredAuctions()
        {
            var expired = _auctions.Values
                .Where(a => DateTime.UtcNow >= a.EndTime)
                .ToList();

            foreach (var auction in expired)
            {
                if (_auctions.TryRemove(auction.Id, out var removed))
                {
                    bool sold = removed.Close();
                    lock (_lock)
                    {
                        if (sold)
                            _soldAuctions.Add(removed);
                        else
                            _unsoldAuctions.Add(removed);
                    }
                }
            }
        }


        // ---------- Find auktion ----------
        public Auction FindAuctionWithID(int auctionId)
        {
            if (_auctions.TryGetValue(auctionId, out var auction))
                return auction;

            lock (_lock)
            {
                return _soldAuctions.FirstOrDefault(a => a.Id == auctionId)
                       ?? _unsoldAuctions.FirstOrDefault(a => a.Id == auctionId)
                       ?? throw new KeyNotFoundException($"Auktion med ID {auctionId} blev ikke fundet.");
            }
        }
    }
}
