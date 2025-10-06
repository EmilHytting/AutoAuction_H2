using System;

namespace AutoAuction_H2.Models.Entities
{

    public class PrivateCar : Car
    {
        public bool Isofix { get; init; }

        public PrivateCar(
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
            int trunkSizeLitres,
            bool hasIsofix
        ) : base(name, regNumber, year, purchasePrice, mileage, towBar, motorSize, fuelEfficiency, fuelType, seats, trunkSizeLitres)
        {
            if (seats < 2 || seats > 7)
                throw new ArgumentException("En privatbil skal have mellem 2 og 7 sæder.");

            Isofix = hasIsofix;
            LicenseType = LicenseType.B;
        }

        private PrivateCar() { } // til EF Core
    }
}
