using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CurrencyConvert.Infrastructure.Serialization
{
    public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
    {
        //TODO: This isn't converting the unix timestamp correctly as is.
        public override DateTimeOffset Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
            DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32());

        public override void Write(
            Utf8JsonWriter writer,
            DateTimeOffset dateTimeValue,
            JsonSerializerOptions options) =>
            writer.WriteStringValue(dateTimeValue.ToUnixTimeSeconds().ToString());
    }
}
