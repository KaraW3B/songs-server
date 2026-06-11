using System.Text.Json;
using KaraWeb.Core.Persistence.Models.Songs;
using KaraWeb.Shared.Helpers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KaraWeb.Core.Persistence.Converters
{
    internal sealed class SongMedleyValueConverter : ValueConverter<SongMedley, string>
    {
        public SongMedleyValueConverter() : base(
            s => s == null ? null : JsonSerializer.Serialize(s, JsonHelper.DefaultJsonSerializerOptions),
            s => string.IsNullOrEmpty(s)
                ? null
                : JsonSerializer.Deserialize<SongMedley>(s, JsonHelper.DefaultJsonSerializerOptions)) {}
    }
}
