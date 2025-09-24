using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuction_H2.Models
{
    public abstract class Car : Vehicle
    {
        private int _numberOfSeats;
        public int NumberOfSeats
        {
            get => _numberOfSeats;
            protected set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(NumberOfSeats), "Number of seats must be positive");
                _numberOfSeats = value;
            }
        }

        private int _trunkDimensions;
        public int TrunkDimensions
        {
            get => _trunkDimensions;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(TrunkDimensions), "Trunk volume cannot be negative");
                _trunkDimensions = value;
            }
        }

        private int _loadCapacityKg;
        public int LoadCapacityKg
        {
            get => _loadCapacityKg;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(LoadCapacityKg), "Load capacity must be non-negative");
                _loadCapacityKg = value;
            }
        }

        protected Car(string name, string regNumber, int year, double purchasePrice,
                      double fuelEfficiency, FuelType fuelType, double motorSize)
            : base(name, regNumber, year, purchasePrice, fuelEfficiency, fuelType, motorSize, LicenseType.B)
        {
        }

        protected Car() { }




        public override string ToString()
        {
            return base.ToString() + $", Seats: {NumberOfSeats}, Trunk: {TrunkDimensions} L, Load: {LoadCapacityKg} kg";
        }
    }
}