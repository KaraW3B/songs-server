using System.Collections.Generic;
using System.Linq;
using System.Threading;
using KaraWeb.Core;
using KaraWeb.Core.Models.Collections;
using KaraWeb.Core.Models.Songs;

namespace KaraWeb.Host.Providers.Songs
{
    internal sealed class SongsProvider : ISongsProvider
    {
        private readonly KaraWebDbContext _dbContext;

        public SongsProvider(KaraWebDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IAsyncEnumerable<Song> GetSongsByCollection(Collection collection, CancellationToken cancellationToken)
        {
            return _dbContext.Songs.Where(s => s.CollectionId == collection.Id).ToAsyncEnumerable();
        }
    }
}
