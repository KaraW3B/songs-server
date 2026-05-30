using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using KaraWeb.Core.Models.Collections;
using KaraWeb.Core.Models.Jobs;
using KaraWeb.Core.Services.SongParser;

namespace KaraWeb.Core.Services.CollectionsAnalyzer
{
    public sealed class CollectionsAnalyzerService : ICollectionsAnalyzerService
    {
        private readonly ISongParserService _songParserService;
        private readonly KaraWebDbContext _dbContext;

        public CollectionsAnalyzerService(ISongParserService songParserService, KaraWebDbContext dbContext)
        {
            _songParserService = songParserService;
            _dbContext = dbContext;
        }

        public async Task<Job> StartCollectionAnalyzeAsync(Collection collection, CancellationToken cancellationToken)
        {
            // TODO: Make it background
            var job = new Job { JobId = Guid.NewGuid(), Status = JobStatus.Processing };

            var directory = new DirectoryInfo(collection.Path);
            if (!directory.Exists)
            {
                job.Status = JobStatus.Error;
                job.ResultMessage = $"Directory '{directory.FullName}' doesn't exist";
                return job;
            }

            await Parallel.ForEachAsync(directory.GetFiles("*.txt", SearchOption.AllDirectories), cancellationToken, (f, c) => ParseSongFile(collection.Id, f, c));
            job.ResultMessage = $"Collection {collection.Name} parsed successfully";
            return job;
        }

        private async ValueTask ParseSongFile(int collectionId, FileInfo songFile, CancellationToken cancellationToken)
        {
            var parsingResult = await _songParserService.ParseSongAsync(collectionId, songFile, cancellationToken);

            if (parsingResult.IsSuccess)
            {
                await _dbContext.Songs.AddAsync(parsingResult.ParsedSong, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
