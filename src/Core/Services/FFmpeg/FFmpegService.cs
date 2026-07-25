using FFMpegCore;
using FFMpegCore.Arguments;
using KaraW3B.Server.Songs.Core.Helpers;
using KaraW3B.Server.Songs.Core.Services.Settings;
using System.Threading;
using System.Threading.Tasks;
using KaraW3B.Server.Songs.Models.Songs.Files;

namespace KaraW3B.Server.Songs.Core.Services.FFmpeg
{
    public sealed class FFmpegService : IFFmpegService
    {
        private const string EncodedByTag = "encoded_by";

        public FFmpegService(ISettingsService settingsService)
        {
            var customFFmpegPath = settingsService.Settings.FFmpegPath;
            if (!string.IsNullOrEmpty(customFFmpegPath))
            {
                GlobalFFOptions.Configure(options => options.BinaryFolder = customFFmpegPath);
            }
        }

        public async Task<FFProbeInfo> GetFileInfo(string filePath, bool audio, CancellationToken cancellationToken)
        {
            var mediaInfos = await FFProbe.AnalyseAsync(filePath, cancellationToken: cancellationToken);

            MediaStream stream = audio ? mediaInfos.PrimaryAudioStream : mediaInfos.PrimaryVideoStream;
            var songFile = new FFProbeInfo
            {
                Format = mediaInfos.Format.FormatName.ToUpperInvariant(),
                Codec = stream?.CodecName.ToUpperInvariant()
            };

            if (mediaInfos.Format.Tags?.TryGetValue(EncodedByTag, out var encodedBy) ?? false)
            {
                songFile.EncodedBy = encodedBy;
            }

            return songFile;
        }
    }
}
