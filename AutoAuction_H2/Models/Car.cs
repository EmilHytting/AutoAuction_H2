using System;

namespace AutoAuction_H2.Models
{
    public abstract class Car : Vehicle
    {
        public int Seats { get; private set; }
        public TrunkDimensions Trunk { get; private set; }

        protected Car(
            string name,
            string regNumber,
            int year,
            decimal purchasePrice,
            double mileage,
            bool towBar,
            double motorSize,
            double fuelEfficiency,
            FuelType fuelType,
            int seats,
            TrunkDimensions trunk)
            : base(name, regNumber, year, purchasePrice, mileage, towBar, motorSize, fuelEfficiency, fuelType)
        {
            if (motorSize < 0.7 || motorSize > 10.0)
                throw new ArgumentOutOfRangeException(nameof(motorSize), "Motorstørrelsen for en personbil skal være mellem 0,7 og 10,0 liter.");

            Seats = seats;
            Trunk = trunk;
            LicenseType = LicenseType.B;
        }

        protected Car() { }

        public override string ToString()
        {
            return base.ToString() + $" | Personbil: {Seats} sæder, Bagagerum: {Trunk}";
        }
    }

    public record TrunkDimensions(double Length, double Width, double Height)
    {
        public override string ToString() => $"{Length}x{Width}x{Height} cm";
    }
}
