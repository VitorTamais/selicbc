using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SelicBC.Auxiliares.Conversores
{
    public class ConversorData : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader r, Type t, JsonSerializerOptions o)
          => DateTime.ParseExact(r.GetString()!, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        public override void Write(Utf8JsonWriter w, DateTime v, JsonSerializerOptions o)
          => w.WriteStringValue(v.ToString("dd/MM/yyyy"));
    }
}
