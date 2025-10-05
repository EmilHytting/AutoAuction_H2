using System;
namespace AutoAuction_H2.Models.Entities
{
    public abstract class Car : Vehicle
    {
        public int Seats { get; private set; }
        public int TrunkSize { get; private set; }

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
            int trunkSizeLitres)   
            : base(name, regNumber, year, purchasePrice, mileage, towBar, motorSize, fuelEfficiency, fuelType)
        {
            if (motorSize < 0.7 || motorSize > 10.0)
                throw new ArgumentOutOfRangeException(nameof(motorSize),
                    "Motorstørrelsen for en personbil skal være mellem 0,7 og 10,0 liter.");

            if (trunkSizeLitres < 0)
                throw new ArgumentOutOfRangeException(nameof(trunkSizeLitres), "Bagagerum kan ikke være negativt.");

            Seats = seats;
            TrunkSize = trunkSizeLitres;
            LicenseType = LicenseType.B;
        }

        protected Car() { }

        public override string ToString()
        {
            return base.ToString() + $" | Personbil: {Seats} sæder, Bagagerum: {TrunkSize} L";
        }
    }
}
