using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class StringToDoubleConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (double.TryParse(reader.GetString(), out double value))
            {
                return value;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDouble();
        }

        throw new JsonException($"Unable to convert {reader.GetString()} to {typeToConvert}");
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
