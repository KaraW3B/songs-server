using System;
using KaraWeb.Core;
using KaraWeb.Core.Models.Collections;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KaraWeb.Core.Models.Jobs;
using KaraWeb.Core.Services.CollectionsAnalyzer;
using KaraWeb.Host.Models.Collections;

namespace KaraWeb.Host.Providers.Collections
{
    internal sealed class CollectionsProvider : ICollectionsProvider
    {
        private readonly KaraWebDbContext _dbContext;
        private readonly ICollectionsAnalyzerService _collectionsAnalyzerService;

        public CollectionsProvider(KaraWebDbContext dbContext, ICollectionsAnalyzerService collectionsAnalyzerService)
        {
            _dbContext = dbContext;
            _collectionsAnalyzerService = collectionsAnalyzerService;
        }

        public IAsyncEnumerable<Collection> GetCollectionsAsync(CancellationToken cancellationToken)
        {
            return _dbContext.Collections.ToAsyncEnumerable();
        }

        public Task<Collection> GetCollectionAsync(Guid collectionId, CancellationToken cancellationToken)
        {
            return _dbContext.Collections.SingleOrDefaultAsync(c => c.Id == collectionId, cancellationToken: cancellationToken);
        }

        public async Task<Collection> CreateCollectionAsync(CollectionPayload payload, CancellationToken cancellationToken)
        {
            var newCollection = new Collection
            {
                Name = payload.Name,
                Description = payload.Description,
                Path = payload.Path
            };
            var collectionEntry = await _dbContext.Collections.AddAsync(newCollection, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return collectionEntry.Entity;
        }

        public async Task DeleteCollectionAsync(Collection collection, CancellationToken cancellationToken)
        {
            _dbContext.Collections.Remove(collection);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<Job> StartCollectionAnalyzeAsync(Collection collection, CollectionAnalyzeType collectionAnalyzeType, CancellationToken cancellationToken)
        {
            return _collectionsAnalyzerService.StartCollectionAnalyzeAsync(collection, collectionAnalyzeType, cancellationToken);
;        }
    }
}
