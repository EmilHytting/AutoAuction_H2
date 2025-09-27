using System;


namespace AutoAuction_H2.Models
{


    public enum FuelType { Benzin, Diesel }
    public enum EnergyClass { A, B, C, D }
    public enum LicenseType { B, BE, C, CE, D, DE }

    public abstract class Vehicle
    {
        public int ID { get; private set; }
        public string? Name { get; private set; }
        public string? RegistrationNumber { get; private set; }
        public int Year { get; private set; }
        public double PurchasePrice { get; private set; }
        public double Mileage { get; set; }
        public bool TowBar { get; set; }
        public LicenseType LicenseType { get; set; }
        public double FuelEfficiency { get; set; }
        public double MotorSize { get; private set; }
        public FuelType FuelType { get; private set; }

        protected Vehicle(string name, string registrationNumber, int year, double purchasePrice,
                          double fuelEfficiency, FuelType fuelType, double motorSize,
                          LicenseType licenseType, double mileage = 0, bool towBar = false)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            RegistrationNumber = registrationNumber ?? throw new ArgumentNullException(nameof(registrationNumber));

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(year, nameof(year));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(purchasePrice, nameof(purchasePrice));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(fuelEfficiency, nameof(fuelEfficiency));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(motorSize, nameof(motorSize));



            Year = year;
            PurchasePrice = purchasePrice;
            FuelEfficiency = fuelEfficiency;
            FuelType = fuelType;
            MotorSize = motorSize;
            LicenseType = licenseType;
            Mileage = mileage;
            TowBar = towBar;
        }

        protected Vehicle() { }

        public EnergyClass GetEnergyClass()
        {
            return FuelType switch
            {
                FuelType.Diesel => FuelEfficiency switch
                {
                    >= 20 => EnergyClass.A,
                    >= 15 and < 20 => EnergyClass.B,
                    < 15 => EnergyClass.C,
                    _ => throw new ArgumentException("Invalid km/l value")
                },
                FuelType.Benzin => FuelEfficiency switch
                {
                    >= 20 => EnergyClass.A,
                    >= 16 and < 20 => EnergyClass.B,
                    >= 12 and < 16 => EnergyClass.C,
                    < 12 => EnergyClass.D,
                    _ => throw new ArgumentException("Invalid km/l value")
                },
                _ => throw new ArgumentException("Invalid fuel type")
            };
        }

        public override string ToString()
        {
            return $"ID: {ID}, Name: {Name} ({Year}), Fuel: {FuelType}, Km/l: {FuelEfficiency}, Energy: {GetEnergyClass()}, License: {LicenseType}, TowBar: {TowBar}";
        }


     
    }



}
