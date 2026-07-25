using System.Text.Json;
using KaraW3B.Server.Songs.Models.Helpers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KaraW3B.Server.Songs.Core.Persistence.Converters
{
    internal class JsonValueConverter<TValue> : ValueConverter<TValue, string> where TValue : class
    {
        public JsonValueConverter() : base(
            s => s == null ? null : JsonSerializer.Serialize(s, JsonHelper.DefaultJsonSerializerOptions),
            s => string.IsNullOrEmpty(s)
                ? null
                : JsonSerializer.Deserialize<TValue>(s, JsonHelper.DefaultJsonSerializerOptions))
        {
        }
    }
}
