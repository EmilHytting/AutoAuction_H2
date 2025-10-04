using System;

namespace AutoAuction_H2.Models
{
    public class PrivateCar : Vehicle
    {
        public int Seats { get; private set; }
        public TrunkDimensions Trunk { get; private set; }
        public bool Isofix { get; private set; }

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
            TrunkDimensions trunk,
            bool isofix
        ) : base(name, regNumber, year, purchasePrice, mileage, towBar, motorSize, fuelEfficiency, fuelType)
        {
            if (seats < 2 || seats > 7)
                throw new ArgumentException("Privatbiler kan have mellem 2 og 7 sæder.");

            Seats = seats;
            Trunk = trunk ?? throw new ArgumentNullException(nameof(trunk));
            Isofix = isofix;
            LicenseType = LicenseType.B;
        }

        private PrivateCar() { } // til EF Core
    }
}
