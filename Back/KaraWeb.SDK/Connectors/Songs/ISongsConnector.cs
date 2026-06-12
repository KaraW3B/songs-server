using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using KaraWeb.Shared.Models.Songs;
using KaraWeb.Shared.Models.Songs.Files;

namespace KaraWeb.SDK.Connectors.Songs
{
    public interface ISongsConnector
    {
        Task<DetailedSongDto> GetSongDetailsAsync(Guid songId, CancellationToken cancellationToken = default);

        Task<Stream> GetSongFileStreamAsync(Guid songId, FileType fileType,
            CancellationToken cancellationToken = default);
    }
}