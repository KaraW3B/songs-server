using System.Collections.Generic;
using System.Threading;
using KaraWeb.Core.Models.Collections;
using KaraWeb.Core.Models.Songs;

namespace KaraWeb.Host.Providers.Songs
{
    public interface ISongsProvider
    {
        IAsyncEnumerable<Song> GetSongsByCollection(Collection collection, CancellationToken cancellationToken);
    }
}
