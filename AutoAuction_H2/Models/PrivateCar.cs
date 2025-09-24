using System;

namespace AutoAuction_H2.Models
{
    public class PrivateCar : Car
    {
        public bool Isofix { get; set; }

        public PrivateCar(string name, string regNumber, int year, double purchasePrice,
                          double fuelEfficiency, FuelType fuelType, double motorSize,
                          int seats, int trunkLitres, bool isofix = false, bool towBar = false)
            : base(name, regNumber, year, purchasePrice, fuelEfficiency, fuelType, motorSize)
        {
            if (seats < 2 || seats > 7)
                throw new ArgumentOutOfRangeException(nameof(seats), "Private cars must have 2-7 seats");

            NumberOfSeats = seats;
            TrunkDimensions = trunkLitres;
            Isofix = isofix;
            TowBar = towBar;
        }

        public PrivateCar() { }

        public override string ToString()
        {
            return "Private Car: " + base.ToString() + $", Isofix: {Isofix}";
        }

    }
}