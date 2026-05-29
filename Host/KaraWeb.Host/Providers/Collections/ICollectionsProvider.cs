using KaraWeb.Core.Models;
using System.Collections.Generic;
using System.Threading;

namespace KaraWeb.Host.Providers.Collections
{
    public interface ICollectionsProvider
    {
        IAsyncEnumerable<Collection> GetCollectionsAsync(CancellationToken cancellationToken); 
    }
}
