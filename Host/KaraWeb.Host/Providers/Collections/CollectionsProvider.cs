using KaraWeb.Core.Models;
using System.Collections.Generic;
using System.Threading;

namespace KaraWeb.Host.Providers.Collections
{
    public class CollectionsProvider : ICollectionsProvider
    {
        public IAsyncEnumerable<Collection> GetCollectionsAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
