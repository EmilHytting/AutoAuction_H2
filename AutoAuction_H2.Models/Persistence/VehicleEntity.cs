using System.Text.Json.Serialization;

namespace AutoAuction_H2.Models.Persistence
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "vehicleType")]
    [JsonDerivedType(typeof(CarEntity), "Car")]
    [JsonDerivedType(typeof(PrivateCarEntity), "PrivateCar")]
    [JsonDerivedType(typeof(ProfessionalCarEntity), "ProfessionalCar")]
    [JsonDerivedType(typeof(TruckEntity), "Truck")]
    [JsonDerivedType(typeof(BusEntity), "Bus")]

    public abstract class VehicleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string RegistrationNumber { get; set; } = null!;
        public int Year { get; set; }
        public decimal PurchasePrice { get; set; }
        public double Mileage { get; set; }
        public bool TowBar { get; set; }
        public double MotorSize { get; set; }
        public double FuelEfficiency { get; set; }
        public int FuelType { get; set; } // Enum mapped as int
    }

    public class CarEntity : VehicleEntity
    {
        public int Seats { get; set; }
        public int TrunkSize { get; set; }
        public bool Isofix { get; set; }
    }

    public class TruckEntity : VehicleEntity
    {
        public double LoadCapacity { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
    }

    public class BusEntity : VehicleEntity
    {
        public double Height { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public int Doors { get; set; }
        public int Seats { get; set; }
        public bool HasToilet { get; set; }
    }

    public class PrivateCarEntity : VehicleEntity
    {
        public int Seats { get; set; }
        public bool Isofix { get; set; }
    }

    public class ProfessionalCarEntity : VehicleEntity
    {
        public int Seats { get; set; }
        public int TrunkSize { get; set; }
        public bool SafetyBar { get; set; }
        public double LoadCapacity { get; set; }
    }
}
