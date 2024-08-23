using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudSalesSystem.HelperClasses
{
    /// <summary>
    /// Converts datetime for json response
    /// </summary>
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var date = reader.GetString();
            return DateTime.Parse(date!, CultureInfo.InvariantCulture);

        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString("dd/MM/yyyy"));
        }
    }
}
