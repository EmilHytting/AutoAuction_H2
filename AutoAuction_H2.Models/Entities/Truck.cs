using System;

namespace AutoAuction_H2.Models.Entities
{
    public class Truck : HeavyVehicle
    {
        public double LoadCapacity { get; private set; }

        internal Truck(
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
            double loadCapacity)
            : base(name, regNumber, year, purchasePrice, mileage, towBar, motorSize, fuelEfficiency, fuelType, height, weight, length)
        {
            if (fuelType != FuelType.Diesel)
                throw new ArgumentException("En lastbil kan kun køre på diesel.");

            LoadCapacity = loadCapacity;
            LicenseType = towBar ? LicenseType.CE : LicenseType.C;
        }

        private Truck() { }

        public override string ToString()
        {
            return base.ToString() + $" | Lastbil: Lasteevne {LoadCapacity} kg";
        }
    }
}
