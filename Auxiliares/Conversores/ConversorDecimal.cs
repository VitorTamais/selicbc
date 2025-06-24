using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SelicBC.Auxiliares.Conversores
{
    public class ConversorDecimal : JsonConverter<decimal>
    {
        private static readonly CultureInfo PtBr = new("pt-BR");
        public override decimal Read(ref Utf8JsonReader r, Type t, JsonSerializerOptions o)
          => decimal.Parse(r.GetString()!, NumberStyles.Number, PtBr);
        public override void Write(Utf8JsonWriter w, decimal v, JsonSerializerOptions o)
          => w.WriteStringValue(v.ToString("F2", PtBr));
    }
}
