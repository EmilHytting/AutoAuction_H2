using System;

namespace AutoAuction_H2.Models
{
public class ProfessionalCar : Car
    {
        public bool HasSafetyBar { get; set; }

        public ProfessionalCar(string name, string regNumber, int year, double purchasePrice,
                               double fuelEfficiency, FuelType fuelType, double motorSize,
                               int seats, int loadCapacityKg, bool towBar = false, bool hasSafetyBar = false)
            : base(name, regNumber, year, purchasePrice, fuelEfficiency, fuelType, motorSize)
        {
            NumberOfSeats = seats;
            LoadCapacityKg = loadCapacityKg;
            TowBar = towBar;
            HasSafetyBar = hasSafetyBar;
        }

        public ProfessionalCar() { }

        public override string ToString()
        {
            return "Professional Car: " + base.ToString() + $", SafetyBar: {HasSafetyBar}";
        }

    }
}

