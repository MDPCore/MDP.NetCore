using System;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MDP.Text.Json
{
    public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
    {
        // Methods
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new TimeSpan(reader.GetInt64());
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Ticks);
        }
    }
}
