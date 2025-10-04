using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoAuction_H2.Models
{
    public class Auction
    {
        public int Id { get; private set; }
        public Vehicle Vehicle { get; private set; }
        public IUser Seller { get; private set; }
        public decimal MinPrice { get; private set; }
        public List<Bid> Bids { get; private set; } = new();
        public bool IsSold { get; private set; } = false;

        public Bid? HighestBid => Bids.OrderByDescending(b => b.Amount).FirstOrDefault();

        private Auction() { } // EF Core

        internal Auction(Vehicle vehicle, IUser seller, decimal minPrice)
        {
            Vehicle = vehicle ?? throw new ArgumentNullException(nameof(vehicle), "Køretøjet må ikke være null.");
            Seller = seller ?? throw new ArgumentNullException(nameof(seller), "Sælger må ikke være null.");
            if (minPrice < 0)
                throw new ArgumentException("Mindsteprisen kan ikke være negativ.");

            MinPrice = minPrice;
        }

        // ---------- FACTORY METHODS ----------
        public static Auction CreatePrivateCarAuction(IUser seller,
            string name, string regNr, int year, decimal price, double mileage,
            bool towBar, double motorSize, double fuelEfficiency, FuelType fuelType,
            int seats, TrunkDimensions trunk, bool isofix, decimal minPrice)
        {
            var car = new PrivateCar(name, regNr, year, price, mileage, towBar, motorSize,
                                     fuelEfficiency, fuelType, seats, trunk, isofix);
            return new Auction(car, seller, minPrice);
        }

        public static Auction CreateProfessionalCarAuction(IUser seller,
            string name, string regNr, int year, decimal price, double mileage,
            bool towBar, double motorSize, double fuelEfficiency, FuelType fuelType,
            int seats, TrunkDimensions trunk, bool safetyBar, double loadCapacity, decimal minPrice)
        {
            var car = new ProfessionalCar(name, regNr, year, price, mileage, towBar, motorSize,
                                          fuelEfficiency, fuelType, seats, trunk, safetyBar, loadCapacity);
            return new Auction(car, seller, minPrice);
        }

        public static Auction CreateTruckAuction(IUser seller,
            string name, string regNr, int year, decimal price, double mileage,
            bool towBar, double motorSize, double fuelEfficiency, FuelType fuelType,
            double height, double weight, double length, double loadCapacity, decimal minPrice)
        {
            var truck = new Truck(name, regNr, year, price, mileage, towBar, motorSize,
                                  fuelEfficiency, fuelType, height, weight, length, loadCapacity);
            return new Auction(truck, seller, minPrice);
        }

        public static Auction CreateBusAuction(IUser seller,
            string name, string regNr, int year, decimal price, double mileage,
            bool towBar, double motorSize, double fuelEfficiency, FuelType fuelType,
            double height, double weight, double length,
            int passengerSeats, int sleepingPlaces, bool toilet, decimal minPrice)
        {
            var bus = new Bus(name, regNr, year, price, mileage, towBar, motorSize,
                              fuelEfficiency, fuelType, height, weight, length, passengerSeats, sleepingPlaces, toilet);
            return new Auction(bus, seller, minPrice);
        }

        // ---------- METHODS ----------
        public bool PlaceBid(IUser bidder, decimal amount)
        {
            if (IsSold)
                throw new InvalidOperationException("Auktionen er allerede solgt.");
            if (HighestBid != null && amount <= HighestBid.Amount)
                throw new ArgumentException("Buddet skal være højere end det nuværende højeste bud.");

            var bid = new Bid(bidder, amount);
            Bids.Add(bid);

            if (amount >= MinPrice)
                Seller.NotifyAboutBid(this, amount);

            return true;
        }

        public bool AcceptBid(IUser seller)
        {
            // Brug brugernavn som identitet (unikt), så wrapper og original er samme ejer
            if (!string.Equals(seller.UserName, Seller.UserName, StringComparison.Ordinal))
                throw new InvalidOperationException("Kun den oprindelige sælger kan acceptere et bud.");
            if (HighestBid == null)
                throw new InvalidOperationException("Der er ingen bud at acceptere.");

            var buyer = HighestBid.Bidder;
            if (!buyer.Withdraw(HighestBid.Amount))
                throw new InvalidOperationException("Køberen har ikke tilstrækkelig saldo.");

            Seller.Deposit(HighestBid.Amount);
            IsSold = true;
            return true;
        }

        // Brug denne i stedet for at sætte Seller direkte
        internal void OverrideSeller(IUser newSeller)
        {
            Seller = newSeller ?? throw new ArgumentNullException(nameof(newSeller));
        }

        public override string ToString()
        {
            string status = IsSold ? "SOLGT" : "Til salg";
            string highest = HighestBid != null ? $"{HighestBid.Amount} kr." : "Ingen bud endnu";
            return $"Auktion #{Id}: {Vehicle.Name} ({Vehicle.Year}) - {status}, Højeste bud: {highest}";
        }
    }
}
