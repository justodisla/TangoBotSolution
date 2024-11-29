using System;
using System.Text.Json;
using TangoBot.API.TTServices;

namespace HttpClientLib.InstrumentApi
{
    public static class InstrumentInfoDeserializer
    {
        public static Instrument DeserializeInstrument(string responseBody)
        {
            try
            {
                var jsonDocument = JsonDocument.Parse(responseBody);
                var root = jsonDocument.RootElement;

                if (root.TryGetProperty("data", out JsonElement dataElement))
                {
                    var instrument = JsonSerializer.Deserialize<Instrument>(dataElement.GetRawText());
                    return instrument;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception during deserialization: {ex.Message}");
                return null;
            }
        }
    }
}
