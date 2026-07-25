using KaraW3B.Server.Songs.Models.Songs.Files;

namespace KaraW3B.Server.Songs.Core.Persistence.Models.Songs
{
    public sealed class DbSongFile
    {
        public string FilePath { get; set; }

        public FFProbeInfo FFProbeInfo { get; set; }

        public SongFileInfo ToSongFileInfo()
        {
            return new SongFileInfo
            {
                FilePath = FilePath,
                FFProbeInfo = FFProbeInfo?.Clone()
            };
        }
    }
}
