using System.Collections.Generic;

namespace KaraWeb.Shared.Models.Songs
{
    public interface IAnalyzableSong
    {
        string Encoding { get; }
        string Version { get; }
        string Title { get; }
        string Artist { get; }
        string Audio { get; }
        double? Gap { get; }
        double? Start { get; }
        double? End { get; }
        string Video { get; }
        double? VideoGap { get; }
        double? PreviewStart { get; }
        string Cover { get; }
        string Background { get; }
        double? Bpm { get; }
        int? MedleyStart { get; }
        int? MedleyEnd { get; }
        string AudioUrl { get; }
        string VideoUrl { get; }
        string CoverUrl { get; }
        string BackgroundUrl { get; }
        List<string> Languages { get; }
        Dictionary<int, string> GetPlayers();
    }
}