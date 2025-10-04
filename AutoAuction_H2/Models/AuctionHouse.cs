using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoAuction_H2.Services;

namespace AutoAuction_H2.Models
{
    public class AuctionHouse
    {
        private readonly ConcurrentDictionary<int, Auction> _auctions = new();
        private readonly List<Auction> _soldAuctions = new();
        private int _auctionCounter = 0;
        private readonly object _lock = new();

        private readonly IUserService _userService;

        public AuctionHouse(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        // Convenience ctor (valgfri)
        public AuctionHouse() : this(new UserService()) { }

        public IEnumerable<Auction> ActiveAuctions => _auctions.Values;
        public IEnumerable<Auction> SoldAuctions => _soldAuctions;

        // ---------- Sæt til salg ----------
        public int SetForSale(Vehicle vehicle, IUser seller, decimal minPrice)
        {
            if (_userService.FindByUserName(seller.UserName) == null)
                throw new InvalidOperationException("Sælgeren er ikke registreret i systemet.");

            var auction = new Auction(vehicle, seller, minPrice);
            int auctionId = Interlocked.Increment(ref _auctionCounter);
            auction.GetType().GetProperty("Id")?.SetValue(auction, auctionId);

            if (!_auctions.TryAdd(auctionId, auction))
                throw new InvalidOperationException("Kunne ikke tilføje auktion.");

            return auctionId;
        }

        // Overload med delegate callback
        public int SetForSale(Vehicle vehicle, IUser seller, decimal minPrice, Action<Auction, decimal> notificationMethod)
        {
            var auctionId = SetForSale(vehicle, seller, minPrice);
            var auction = _auctions[auctionId];

            // ⬇️ Brug metode – ikke direkte set af Seller
            auction.OverrideSeller(new DelegateUserWrapper(seller, notificationMethod));

            return auctionId;
        }

        // ---------- Modtag bud ----------
        public bool ReceiveBid(IUser buyer, int auctionId, decimal bid)
        {
            if (_userService.FindByUserName(buyer.UserName) == null)
                throw new InvalidOperationException("Køberen er ikke registreret i systemet.");

            if (!_auctions.TryGetValue(auctionId, out var auction))
                throw new KeyNotFoundException($"Auktion med ID {auctionId} blev ikke fundet.");

            return auction.PlaceBid(buyer, bid);
        }

        // ---------- Accepter bud ----------
        public bool AcceptBid(IUser seller, int auctionId)
        {
            if (_userService.FindByUserName(seller.UserName) == null)
                throw new InvalidOperationException("Sælgeren er ikke registreret i systemet.");

            if (!_auctions.TryRemove(auctionId, out var auction))
                throw new KeyNotFoundException($"Auktion med ID {auctionId} blev ikke fundet.");

            bool success = auction.AcceptBid(seller);

            if (success)
            {
                lock (_lock)
                {
                    _soldAuctions.Add(auction);
                }
            }
            else
            {
                _auctions[auctionId] = auction;
            }

            return success;
        }

        // ---------- Find auktion med ID ----------
        public Auction FindAuctionWithID(int auctionId)
        {
            if (_auctions.TryGetValue(auctionId, out var auction))
                return auction;

            lock (_lock)
            {
                return _soldAuctions.FirstOrDefault(a => a.Id == auctionId)
                       ?? throw new KeyNotFoundException($"Auktion med ID {auctionId} blev ikke fundet.");
            }
        }
    }

    /// <summary>
    /// Wrapper, der kun ændrer notifikation – men sørger for at penge lander på den rigtige konto.
    /// </summary>
    internal class DelegateUserWrapper : User
    {
        private readonly IUser _innerUser;
        private readonly Action<Auction, decimal> _notification;

        public DelegateUserWrapper(IUser innerUser, Action<Auction, decimal> notification)
            : base(innerUser.UserName, "dummy", innerUser.ZipCode, innerUser.Balance, innerUser.UserType)
        {
            _innerUser = innerUser;
            _notification = notification;
        }

        public override void NotifyAboutBid(Auction auction, decimal amount)
        {
            _notification.Invoke(auction, amount);
        }

        // Videresend økonomi til den rigtige bruger + synkronisér wrapperens Balance
        public override bool Withdraw(decimal amount)
        {
            var ok = _innerUser.Withdraw(amount);
            Balance = _innerUser.Balance;
            return ok;
        }

        public override void Deposit(decimal amount)
        {
            _innerUser.Deposit(amount);
            Balance = _innerUser.Balance;
        }
    }
}
