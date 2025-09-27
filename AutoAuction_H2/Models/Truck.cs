    namespace AutoAuction_H2.Models
    {
        public class Truck : HeavyVehicle
        {
            public double LoadCapacityKg { get; set; }

            public Truck(string name, string regNumber, int year, double purchasePrice,
                           double fuelEfficiency, FuelType fuelType, double motorSize,
                           double height, double weight, double length,
                           double loadCapacityKg, bool towBar = false)
                : base(name, regNumber, year, purchasePrice, fuelEfficiency, fuelType, motorSize,
                       height, weight, length, towBar)
            {
                LoadCapacityKg = loadCapacityKg;
                LicenseType = towBar ? LicenseType.CE : LicenseType.C;
            }

            public Truck() { }

            public override string ToString()
            {
                return "Truck: " + base.ToString() + $", Load Capacity: {LoadCapacityKg} kg";
            }

        }
    }

