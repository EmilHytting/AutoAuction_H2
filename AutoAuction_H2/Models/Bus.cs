using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuction_H2.Models
{
    public class Bus : HeavyVehicle
    {
        public int SeatCount { get; set; }
        public int SleepingCount { get; set; }
        public bool Toilet { get; set; }

        public Bus(string name, string regNumber, int year, double purchasePrice,
                   double fuelEfficiency, FuelType fuelType, double motorSize,
                   double height, double weight, double length,
                   int seatCount, int sleepingCount, bool toilet, bool towBar = false)
            : base(name, regNumber, year, purchasePrice, fuelEfficiency, fuelType, motorSize,
                   height, weight, length, towBar)
        {
            SeatCount = seatCount;
            SleepingCount = sleepingCount;
            Toilet = toilet;

            LicenseType = towBar ? LicenseType.DE : LicenseType.D;
        }

        public Bus() { }

        public override string ToString()
        {
            return "Bus: " + base.ToString() + $", Seats: {SeatCount}, Sleepers: {SleepingCount}, Toilet: {Toilet}";
        }


    }
}
