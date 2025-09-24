using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuction_H2.Models
{
    public abstract class HeavyVehicle : Vehicle
    {
        private double _height;
        public double Height
        {
            get => _height;
            set => _height = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof(Height));
        }

        private double _weight;
        public double Weight
        {
            get => _weight;
            set => _weight = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof(Weight));
        }

        private double _length;
        public double Length
        {
            get => _length;
            set => _length = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof(Length));
        }

        protected HeavyVehicle(string name, string regNumber, int year, double purchasePrice,
                               double fuelEfficiency, FuelType fuelType, double motorSize,
                               double height, double weight, double length, bool towBar = false)
            : base(name, regNumber, year, purchasePrice, fuelEfficiency, fuelType, motorSize,
                   LicenseType.C, 0, towBar) // Default license, overridden in subclasses
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(motorSize, nameof(motorSize));
            if (motorSize < 4.2 || motorSize > 15.0)
                throw new ArgumentOutOfRangeException(nameof(motorSize), "Motor size must be between 4.2 and 15 L");

            Height = height;
            Weight = weight;
            Length = length;
        }

        protected HeavyVehicle() { }

        public override string ToString()
        {
            return base.ToString() + $", H: {Height:F1} m, W: {Weight:N0} kg, L: {Length:F1} m";
        }

    }
}
