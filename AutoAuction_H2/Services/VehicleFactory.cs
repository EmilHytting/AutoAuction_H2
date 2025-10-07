using AutoAuction_H2.Models.Persistence;
using AutoAuction_H2.ViewModels;
using System;

namespace AutoAuction_H2.Services
{
    public class VehicleFactory
    {
        public VehicleEntity Create(string? type, CreateAuctionViewModel vm)
        {
            return type switch
            {
                "Bil" => new CarEntity
                {
                    Name = vm.Name,
                    RegistrationNumber = vm.RegNumber,
                    Year = vm.Year ?? 0,
                    Mileage = vm.Mileage ?? 0,
                    TowBar = vm.TowBar,
                    MotorSize = vm.MotorSize ?? 0,
                    FuelEfficiency = vm.FuelEfficiency ?? 0,
                    FuelType = vm.FuelType,
                    Seats = vm.Seats ?? 0,
                    TrunkSize = vm.TrunkSize ?? 0,
                    Isofix = vm.Isofix
                },
                "Truck" => new TruckEntity
                {
                    Name = vm.Name,
                    RegistrationNumber = vm.RegNumber,
                    Year = vm.Year ?? 0,
                    Mileage = vm.Mileage ?? 0,
                    TowBar = vm.TowBar,
                    MotorSize = vm.MotorSize ?? 0,
                    FuelEfficiency = vm.FuelEfficiency ?? 0,
                    FuelType = vm.FuelType,
                    LoadCapacity = vm.LoadCapacity ?? 0,
                    Height = vm.Height ?? 0,
                    Length = vm.Length ?? 0,
                    Width = vm.Width ?? 0
                },
                "Bus" => new BusEntity
                {
                    Name = vm.Name,
                    RegistrationNumber = vm.RegNumber,
                    Year = vm.Year ?? 0,
                    Mileage = vm.Mileage ?? 0,
                    TowBar = vm.TowBar,
                    MotorSize = vm.MotorSize ?? 0,
                    FuelEfficiency = vm.FuelEfficiency ?? 0,
                    FuelType = vm.FuelType,
                    Height = vm.Height ?? 0,
                    Length = vm.Length ?? 0,
                    Width = vm.Width ?? 0,
                    Seats = vm.Seats ?? 0,
                    Doors = vm.Doors ?? 0,
                    HasToilet = vm.HasToilet
                },
                "Varevogn" => new ProfessionalCarEntity
                {
                    Name = vm.Name,
                    RegistrationNumber = vm.RegNumber,
                    Year = vm.Year ?? 0,
                    Mileage = vm.Mileage ?? 0,
                    TowBar = vm.TowBar,
                    MotorSize = vm.MotorSize ?? 0,
                    FuelEfficiency = vm.FuelEfficiency ?? 0,
                    FuelType = vm.FuelType,
                    Seats = vm.Seats ?? 0,
                    TrunkSize = vm.TrunkSize ?? 0,
                    SafetyBar = vm.SafetyBar,
                    LoadCapacity = vm.LoadCapacity ?? 0
                },
                _ => throw new InvalidOperationException("Ukendt køretøjstype")
            };
        }
    }
}
