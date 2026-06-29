using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using KaraWeb.Shared.Models.Songs;
using KaraWeb.Shared.Models.Songs.Files;
using KaraWeb.Shared.Models.Songs.Messages;
using KaraWeb.Shared.Models.Songs.Notes;

namespace KaraWeb.SDK.Connectors.Songs
{
    public interface ISongsConnector
    {
        Task<SongDto> GetSongAsync(Guid songId, CancellationToken cancellationToken = default);
        IAsyncEnumerable<SongNoteDto> GetSongNotesAsync(Guid songId, CancellationToken cancellationToken = default);
        IAsyncEnumerable<SongAlertDto> GetSongAlertsAsync(Guid songId, CancellationToken cancellationToken = default);

        Task<Stream> GetSongFileStreamAsync(Guid songId, FileType fileType,
            CancellationToken cancellationToken = default);
    }
}