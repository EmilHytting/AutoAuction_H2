using System;
using System.Text.RegularExpressions;

namespace AutoAuction_H2.Models.Entities
{
    public enum FuelType { Benzin, Diesel, Electric, Hydrogen }
    public enum LicenseType { A, B, BE, C, CE, D, DE }
    public enum EnergyClass { A, B, C, D }

    public abstract class Vehicle
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        private string _registrationNumber = null!;
        public string RegistrationNumber
        {
            get => MaskRegistration(_registrationNumber);   // returnerer altid maskeret
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Registreringsnummer må ikke være tomt.");

                if (!Regex.IsMatch(value, @"^[A-Za-z]{2,3}\d{4,5}$"))
                    throw new ArgumentException("Registreringsnummer skal bestå af 2-3 bogstaver og 4-5 cifre.");


                _registrationNumber = value;
            }
        }

        public int Year { get; private set; }
        public decimal PurchasePrice { get; private set; }
        public double Mileage { get; private set; }
        public bool TowBar { get; private set; }
        public LicenseType LicenseType { get; protected set; }
        public double MotorSize { get; private set; }
        public double FuelEfficiency { get; private set; }
        public FuelType FuelType { get; private set; }

        // 🔹 Tilføj denne property
        public int TrunkSize { get; private set; }

        public EnergyClass EnergyClass => GetEnergyClass();

        // ---------- CONSTRUCTORS ----------
        protected Vehicle(
            string name,
            string regNumber,
            int year,
            decimal purchasePrice,
            double mileage,
            bool towBar,
            double motorSize,
            double fuelEfficiency,
            FuelType fuelType
            )
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Navn må ikke være tomt.");
            if (mileage < 0)
                throw new ArgumentException("Kilometerstand må ikke være negativ.");
            if (purchasePrice < 0)
                purchasePrice = 0;

            Name = name;
            RegistrationNumber = regNumber;
            Year = year;
            PurchasePrice = purchasePrice;
            Mileage = mileage;
            TowBar = towBar;
            MotorSize = motorSize;
            FuelEfficiency = fuelEfficiency;
            FuelType = fuelType;
        }

        protected Vehicle() { }

        // ---------- METHODS ----------
        public EnergyClass GetEnergyClass()
        {
            if (FuelType == FuelType.Electric || FuelType == FuelType.Hydrogen)
                return EnergyClass.A;

            if (Year < 2010)
            {
                return FuelType == FuelType.Diesel
                    ? FuelEfficiency >= 23 ? EnergyClass.A :
                       FuelEfficiency >= 18 ? EnergyClass.B :
                       FuelEfficiency >= 13 ? EnergyClass.C : EnergyClass.D
                    : FuelEfficiency >= 18 ? EnergyClass.A :
                       FuelEfficiency >= 14 ? EnergyClass.B :
                       FuelEfficiency >= 10 ? EnergyClass.C : EnergyClass.D;
            }
            else
            {
                return FuelType == FuelType.Diesel
                    ? FuelEfficiency >= 25 ? EnergyClass.A :
                       FuelEfficiency >= 20 ? EnergyClass.B :
                       FuelEfficiency >= 15 ? EnergyClass.C : EnergyClass.D
                    : FuelEfficiency >= 20 ? EnergyClass.A :
                       FuelEfficiency >= 16 ? EnergyClass.B :
                       FuelEfficiency >= 12 ? EnergyClass.C : EnergyClass.D;
            }
        }

        private static string MaskRegistration(string regNr)
        {
            if (regNr.Length == 7)
                return $"**{regNr.Substring(2, 3)}**";
            return regNr;
        }

        public override string ToString()
        {
            return $"{Name} ({Year}) - Reg.nr.: {RegistrationNumber}, {Mileage} km, Brændstof: {FuelType}, " +
                   $"Energiklasse: {EnergyClass}, Bagagerum: {TrunkSize} L";
        }
    }
}
