//using AutoAuction_H2.Models;
//using System;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//public class VehicleConverter : JsonConverter<Vehicle>
//{
//    public override Vehicle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//    {
//        using var jsonDoc = JsonDocument.ParseValue(ref reader);
//        var root = jsonDoc.RootElement;

//        if (!root.TryGetProperty("vehicleType", out var typeProp))
//            throw new JsonException("Missing vehicleType property");

//        string vehicleType = typeProp.GetString()!;
//        Vehicle vehicle = vehicleType switch
//        {
//            "Truck" => JsonSerializer.Deserialize<Truck>(root.GetRawText(), options)!,
//            "Bus" => JsonSerializer.Deserialize<Bus>(root.GetRawText(), options)!,
//            "PrivateCar" => JsonSerializer.Deserialize<PrivateCar>(root.GetRawText(), options)!,
//            "ProfessionalCar" => JsonSerializer.Deserialize<ProfessionalCar>(root.GetRawText(), options)!,
//            _ => throw new JsonException($"Unknown vehicleType: {vehicleType}")
//        };

//        return vehicle;
//    }

//    public override void Write(Utf8JsonWriter writer, Vehicle value, JsonSerializerOptions options)
//    {
//        JsonSerializer.Serialize(writer, (object)value, options);
//    }
//}
