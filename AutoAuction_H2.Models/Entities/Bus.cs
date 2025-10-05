using System;

namespace AutoAuction_H2.Models.Entities
{
    public class Bus : HeavyVehicle
    {
        public int PassengerSeats { get; private set; }
        public int SleepingPlaces { get; private set; }
        public bool Toilet { get; private set; }

        internal Bus(
            string name,
            string regNumber,
            int year,
            decimal purchasePrice,
            double mileage,
            bool towBar,
            double motorSize,
            double fuelEfficiency,
            FuelType fuelType,
            double height,
            double weight,
            double length,
            int passengerSeats,
            int sleepingPlaces,
            bool toilet)
            : base(name, regNumber, year, purchasePrice, mileage, towBar, motorSize, fuelEfficiency, fuelType, height, weight, length)
        {
            if (fuelType != FuelType.Diesel)
                throw new ArgumentException("En bus kan kun køre på diesel.");

            PassengerSeats = passengerSeats;
            SleepingPlaces = sleepingPlaces;
            Toilet = toilet;
            LicenseType = towBar ? LicenseType.DE : LicenseType.D;
        }

        private Bus() { }

        public override string ToString()
        {
            return base.ToString() + $" | Bus: {PassengerSeats} siddepladser, {SleepingPlaces} sovepladser, Toilet: {(Toilet ? "Ja" : "Nej")}";
        }
    }
}
