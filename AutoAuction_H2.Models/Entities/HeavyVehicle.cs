using System;

namespace AutoAuction_H2.Models.Entities
{
    public abstract class HeavyVehicle : Vehicle
    {
        public double Height { get; private set; }
        public double Weight { get; private set; }
        public double Length { get; private set; }

        protected HeavyVehicle(
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
            double length)
            : base(name, regNumber, year, purchasePrice, mileage, towBar, motorSize, fuelEfficiency, fuelType)
        {
            if (motorSize < 4.2 || motorSize > 15.0)
                throw new ArgumentOutOfRangeException(nameof(motorSize), "Motorstørrelsen for tunge køretøjer skal være mellem 4,2 og 15,0 liter.");

            Height = height;
            Weight = weight;
            Length = length;
        }

        protected HeavyVehicle() { }
    }
}
