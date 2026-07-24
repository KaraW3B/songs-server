using System.Threading;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Arguments;
using KaraW3B.Server.Songs.Core.Helpers;
using KaraW3B.Server.Songs.Core.Services.Settings;
using System;
using System.Linq;
using KaraW3B.Server.Songs.Models.Songs;

namespace KaraW3B.Server.Songs.Core.Services.FFmpeg
{
    public sealed class FFmpegService : IFFmpegService
    {
        // See https://developer.mozilla.org/en-US/docs/Web/Media/Guides/Formats/Video_codecs
        private const string PreferredVideoFormat = "MP4";
        private const string PreferredVideoCodec = "H264";

        private const string PreferredAudioFormat = "MP3";

        // Probably to enrich
        private static readonly string[] BrowsersVideoSupportedCodecs =
        {
            "H264",
            "H265",
            "AV1",
            "VP9"
        };

        private const string EncodedByTag = "encoded_by";

        public FFmpegService(ISettingsService settingsService)
        {
            var customFFmpegPath = settingsService.Settings.FFmpegPath;
            if (!string.IsNullOrEmpty(customFFmpegPath))
            {
                GlobalFFOptions.Configure(options => options.BinaryFolder = customFFmpegPath);
            }
        }

        public async Task<BrowserCompatibility> GetVideoCompatibilityAsync(string videoPath, CancellationToken cancellationToken)
        {
            var mediaInfos = await FFProbe.AnalyseAsync(videoPath, cancellationToken: cancellationToken);

            if ((mediaInfos.Format.Tags?.TryGetValue(EncodedByTag, out var value) ?? false) &&
                value.StartsWith(KaraW3BConstants.ApplicationName, StringComparison.OrdinalIgnoreCase))
            {
                return BrowserCompatibility.Compatible;
            }

            if (!mediaInfos.Format.FormatName.ToUpperInvariant().Contains(PreferredVideoFormat))
            {
                return BrowserCompatibility.ConversionMandatory;
            }

            var videoStream = mediaInfos.PrimaryVideoStream;
            if (videoStream == null)
            {
                return BrowserCompatibility.ConversionMandatory;
            }

            if (BrowsersVideoSupportedCodecs.Contains(videoStream.CodecName.ToUpperInvariant()))
            {
                return BrowserCompatibility.ConversionMandatory;
            }

            if (videoStream.CodecName.ToUpperInvariant() != PreferredVideoCodec)
            {
                return BrowserCompatibility.ConversionRecommended;
            }

            return BrowserCompatibility.Compatible;
        }

        public async Task<BrowserCompatibility> GetAudioCompatibilityAsync(string audioPath, CancellationToken cancellationToken)
        {
            var mediaInfos = await FFProbe.AnalyseAsync(audioPath, cancellationToken: cancellationToken);

            if ((mediaInfos.Format.Tags?.TryGetValue(EncodedByTag, out var value) ?? false) &&
                value == KaraW3BConstants.ApplicationName)
            {
                return BrowserCompatibility.Compatible;
            }

            if (mediaInfos.Format.FormatName.ToUpperInvariant() != PreferredAudioFormat)
            {
                return BrowserCompatibility.ConversionMandatory;
            }

            var audioStream = mediaInfos.PrimaryAudioStream;
            if (audioStream == null)
            {
                return BrowserCompatibility.ConversionMandatory;
            }

            if (audioStream.CodecName.ToUpperInvariant() != PreferredAudioFormat)
            {
                return BrowserCompatibility.ConversionRecommended;
            }

            return BrowserCompatibility.Compatible;
        }




        private class MovFlagsArgument : IArgument
        {
            private readonly string _flag;

            public MovFlagsArgument(string flag)
            {
                _flag = flag;
            }

            public string Text => $"-movflags {_flag}";
        }

        private class KaraW3BConvertedMetadataArgument : IArgument
        {
            public string Text => $"-metadata {EncodedByTag}=\"{KaraW3BConstants.ApplicationName}-v{KaraW3BConstants.ApplicationVersion}\"";
        }
    }
}
