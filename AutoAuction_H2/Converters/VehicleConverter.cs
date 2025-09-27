using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoAuction_H2.Models;

namespace AutoAuction_H2.Converters;

public class VehicleConverter : JsonConverter<Vehicle>
{
    public override Vehicle? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Parse the current object as JsonDocument
        using var doc = JsonDocument.ParseValue(ref reader);
        var jsonObj = doc.RootElement;

        // Get the type discriminator
        if (!jsonObj.TryGetProperty("vehicleType", out var typeProp))
            throw new JsonException("Missing vehicleType property");

        var typeStr = typeProp.GetString();

        return typeStr switch
        {
            "PrivateCar" => jsonObj.Deserialize<PrivateCar>(options),
            "ProfessionalCar" => jsonObj.Deserialize<ProfessionalCar>(options),
            "Bus" => jsonObj.Deserialize<Bus>(options),
            "Truck" => jsonObj.Deserialize<Truck>(options),
            _ => throw new JsonException($"Unknown vehicleType: {typeStr}")
        };
    }

    public override void Write(Utf8JsonWriter writer, Vehicle value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
    }
}
