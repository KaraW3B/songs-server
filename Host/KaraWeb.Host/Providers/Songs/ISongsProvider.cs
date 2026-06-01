using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KaraWeb.Core.Models.Collections;
using KaraWeb.Core.Models.Songs;

namespace KaraWeb.Host.Providers.Songs
{
    public interface ISongsProvider
    {
        IAsyncEnumerable<Song> GetSongsByCollection(Collection collection, CancellationToken cancellationToken);
        Task<Song> GetSong(Guid songId, CancellationToken cancellationToken);
    }
}
