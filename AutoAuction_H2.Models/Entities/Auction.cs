using AutoAuction_H2.Models.Interfaces;
using System;

namespace AutoAuction_H2.Models.Entities
{
    public class Auction
    {
        public int Id { get; private set; }
        public Vehicle Vehicle { get; private set; }
        public IUser Seller { get; internal set; }
        public IUser? HighestBidder { get; private set; }
        public decimal MinPrice { get; private set; }
        public decimal CurrentBid { get; private set; }
        public bool IsSold { get; private set; }
        public DateTime EndTime { get; internal set; }

        private static readonly TimeSpan DefaultDuration = TimeSpan.FromMinutes(1);
      
        public Auction(Vehicle vehicle, IUser seller, decimal minPrice, TimeSpan? duration = null)
        {
            Vehicle = vehicle ?? throw new ArgumentNullException(nameof(vehicle));
            Seller = seller ?? throw new ArgumentNullException(nameof(seller));
            MinPrice = minPrice;
            CurrentBid = 0m;
            IsSold = false;
            EndTime = DateTime.UtcNow.Add(duration ?? DefaultDuration);
        }

        // ---------- Bidding ----------
        public bool PlaceBid(IUser buyer, decimal bidAmount)
        {
            if (buyer == null) throw new ArgumentNullException(nameof(buyer));
            if (IsSold) return false;
            if (DateTime.UtcNow >= EndTime) return false;
            if (bidAmount <= CurrentBid) return false;

            // Hvis en anden var højeste byder → frigiv deres reservation
            if (HighestBidder != null && HighestBidder != buyer)
            {
                HighestBidder.Release(CurrentBid);
            }

            // Hvis samme byder byder højere → frigiv gammel reservation
            if (HighestBidder == buyer)
            {
                buyer.Release(CurrentBid);
            }

            // Prøv at reservere det nye beløb
            if (!buyer.Reserve(bidAmount))
                return false;

            CurrentBid = bidAmount;
            HighestBidder = buyer;

            // Forlæng tid, hvis vi er tæt på udløb
            if (EndTime - DateTime.UtcNow <= TimeSpan.FromSeconds(30))
                EndTime = DateTime.UtcNow.AddSeconds(30);

            // Kun notify hvis bud >= MinPrice
            if (bidAmount >= MinPrice)
                Seller.NotifyAboutBid(this, bidAmount);

            return true;
        }

        // ---------- Close ----------
        public bool Close()
        {
            if (IsSold) return false; // allerede lukket
            if (HighestBidder == null || CurrentBid < MinPrice)
            {
                IsSold = false;
                return false;
            }

            // Frigiv reservation, før vi trækker endeligt
            HighestBidder.Release(CurrentBid);

            if (!HighestBidder.Withdraw(CurrentBid))
            {
                IsSold = false;
                return false;
            }

            Seller.Deposit(CurrentBid);
            IsSold = true;
            return true;
        }

        // ---------- Factory Methods ----------
        public static Auction CreatePrivateCarAuction(
            IUser seller, string name, string regNr, int year, decimal price, double mileage,
            bool towBar, double motorSize, double fuelEfficiency, FuelType fuelType,
            int seats, int trunkSize, bool isofix, decimal minPrice,
            TimeSpan? duration = null)
        {
            var car = new PrivateCar(name, regNr, year, price, mileage, towBar,
                                     motorSize, fuelEfficiency, fuelType,
                                     seats, trunkSize, isofix);
            return new Auction(car, seller, minPrice, duration ?? DefaultDuration);
        }

        public static Auction CreateProfessionalCarAuction(
            IUser seller, string name, string regNr, int year, decimal price, double mileage,
            bool towBar, double motorSize, double fuelEfficiency, FuelType fuelType,
            int seats, int trunkSize, bool safetyBar, double loadCapacity,
            decimal minPrice, TimeSpan? duration = null)
        {
            var car = new ProfessionalCar(
                name, regNr, year, price, mileage, towBar,
                motorSize, fuelEfficiency, fuelType,
                seats, trunkSize, safetyBar, loadCapacity
            );

            return new Auction(car, seller, minPrice, duration ?? DefaultDuration);
        }


        public static Auction CreateTruckAuction(
            IUser seller, string name, string regNr, int year, decimal price, double mileage,
            bool towBar, double motorSize, double fuelEfficiency, FuelType fuelType,
            double loadCapacity, double height, double length, double width, decimal minPrice,
            TimeSpan? duration = null)
        {
            var truck = new Truck(name, regNr, year, price, mileage, towBar,
                                  motorSize, fuelEfficiency, fuelType,
                                  loadCapacity, height, length, width);
            return new Auction(truck, seller, minPrice, duration ?? DefaultDuration);
        }

        public static Auction CreateBusAuction(
            IUser seller, string name, string regNr, int year, decimal price, double mileage,
            bool towBar, double motorSize, double fuelEfficiency, FuelType fuelType,
            double height, double length, double width, int doors, int seats, bool hasToilet,
            decimal minPrice, TimeSpan? duration = null)
        {
            var bus = new Bus(name, regNr, year, price, mileage, towBar,
                              motorSize, fuelEfficiency, fuelType,
                              height, length, width, doors, seats, hasToilet);
            return new Auction(bus, seller, minPrice, duration ?? DefaultDuration);
        }

    }
}
