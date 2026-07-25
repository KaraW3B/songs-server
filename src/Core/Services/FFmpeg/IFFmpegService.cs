using System.Threading;
using System.Threading.Tasks;
using KaraW3B.Server.Songs.Models.Songs.Files;

namespace KaraW3B.Server.Songs.Core.Services.FFmpeg
{
    public interface IFFmpegService
    {
        public Task<FFProbeInfo> GetFileInfo(string filePath, bool audio, CancellationToken cancellationToken);
    }
}
