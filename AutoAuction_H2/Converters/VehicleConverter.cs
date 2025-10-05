using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoAuction_H2.Models.Entities;

namespace AutoAuction_H2.Converters;

public class VehicleConverter : JsonConverter<Vehicle>
{
    private static string? GetDiscriminator(JsonElement obj)
    {
        // Look for common discriminator names, case-insensitive
        foreach (var prop in obj.EnumerateObject())
        {
            var name = prop.Name;
            if (name.Equals("vehicleType", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("type", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("$type", StringComparison.OrdinalIgnoreCase))
            {
                return prop.Value.ValueKind == JsonValueKind.String ? prop.Value.GetString() : prop.Value.ToString();
            }
        }
        return null;
    }

    private static string? InferTypeFromShape(JsonElement obj)
    {
        // Heuristics based on known properties
        if (obj.TryGetProperty("Isofix", out _))
            return nameof(PrivateCar);
        if (obj.TryGetProperty("HasSafetyBar", out _) || obj.TryGetProperty("LoadCapacityKg", out _))
            return nameof(ProfessionalCar);
        if (obj.TryGetProperty("SeatCount", out _) || obj.TryGetProperty("SleepingCount", out _) || obj.TryGetProperty("Toilet", out _))
            return nameof(Bus);
        // If heavy-vehicle dimensions exist but not bus-specific fields, guess Truck
        if (obj.TryGetProperty("Height", out _) && obj.TryGetProperty("Weight", out _) && obj.TryGetProperty("Length", out _))
            return nameof(Truck);
        return null;
    }

    public override Vehicle? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var jsonObj = doc.RootElement;

        // Try discriminator first
        var typeStr = GetDiscriminator(jsonObj);

        // Some backends might emit a schema placeholder like "string"; treat as unknown
        if (string.IsNullOrWhiteSpace(typeStr) || string.Equals(typeStr, "string", StringComparison.OrdinalIgnoreCase))
        {
            typeStr = InferTypeFromShape(jsonObj);
        }

        // Fallback if still unknown
        typeStr ??= nameof(PrivateCar);

        return typeStr.ToLowerInvariant() switch
        {
            "privatecar" => jsonObj.Deserialize<PrivateCar>(options),
            "professionalcar" => jsonObj.Deserialize<ProfessionalCar>(options),
            "bus" => jsonObj.Deserialize<Bus>(options),
            "truck" => jsonObj.Deserialize<Truck>(options),
            _ => jsonObj.Deserialize<PrivateCar>(options) // safe fallback
        };
    }

    public override void Write(Utf8JsonWriter writer, Vehicle value, JsonSerializerOptions options)
    {
        // Write a discriminator to help consumers
        using var doc = JsonDocument.Parse(JsonSerializer.Serialize(value, value.GetType(), options));
        writer.WriteStartObject();
        writer.WriteString("vehicleType", value.GetType().Name);
        foreach (var prop in doc.RootElement.EnumerateObject())
        {
            prop.WriteTo(writer);
        }
        writer.WriteEndObject();
    }
}
