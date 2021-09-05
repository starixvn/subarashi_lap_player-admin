using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fanex.Athena.PlayerAdmin.Core.JsonConverter
{
    public class StringToDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            const string DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
            writer.WriteStringValue(value.ToString(DateTimeFormat));
        }
    }
}