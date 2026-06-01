using KaraWeb.Core.Models.Collections;
using KaraWeb.Core.Models.Jobs;
using KaraWeb.Core.Services.SongParser;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KaraWeb.Core.Helpers;
using log4net;

namespace KaraWeb.Core.Services.CollectionsAnalyzer
{
    public sealed class CollectionsAnalyzerService : ICollectionsAnalyzerService
    {
        private readonly ILog _logger = LogManager.GetLogger(nameof(CollectionsAnalyzerService));
        private readonly ISongParserService _songParserService;

        public CollectionsAnalyzerService(ISongParserService songParserService)
        {
            _songParserService = songParserService;
        }

        public async Task<Job> StartCollectionAnalyzeAsync(Collection collection, CollectionAnalyzeType analyzeType, CancellationToken cancellationToken)
        {
            // TODO: Make it background
            var job = new Job { JobId = Guid.NewGuid(), Status = JobStatus.Processing };

            var directory = new DirectoryInfo(collection.Path);
            if (!directory.Exists)
            {
                job.Status = JobStatus.Error;
                job.ResultMessage = $"Directory '{directory.FullName}' doesn't exist";
                _logger.Error(job.ResultMessage);
                return job;
            }

            _logger.Info($"Start analyzing collection '{collection.Name}'");
            var timeWatcher = new Stopwatch();
            timeWatcher.Start();

            var parsedSongIds = new ConcurrentBag<Guid>();
            await Parallel.ForEachAsync(directory.GetFiles("*.txt", SearchOption.AllDirectories), cancellationToken, (f, c) => ParseSongFile(collection.Id, analyzeType, parsedSongIds, f, c));

            await using var dbContext = new KaraWebDbContext();

            var songsToDelete = await dbContext.Songs.Where(s => !parsedSongIds.Contains(s.Id)).ToListAsync(cancellationToken);
            dbContext.RemoveRange(songsToDelete);
            await dbContext.SaveChangesAsync(cancellationToken);

            timeWatcher.Stop();
            job.ResultMessage = $"Collection {collection.Name} parsed {parsedSongIds.Count} songs and deleted {songsToDelete.Count} songs successfully in {timeWatcher.Elapsed}";
            _logger.Info(job.ResultMessage);
            return job;
        }

        private async ValueTask ParseSongFile(Guid collectionId, CollectionAnalyzeType analyzeType, ConcurrentBag<Guid> parsedSongIds, FileInfo songFile, CancellationToken cancellationToken)
        {
            _logger.Info($"Starting analyze of song file '{songFile.FullName}'");
            await using var dbContext = new KaraWebDbContext();
            var existingSong = await dbContext.Songs.SingleOrDefaultAsync(s =>
                s.SongFilePath == songFile.FullName &&
                s.CollectionId == collectionId, cancellationToken: cancellationToken);

            var fileHash = await SongHelper.ComputeFileHash(songFile, cancellationToken);
            if (analyzeType == CollectionAnalyzeType.Optimized)
            {
                if (existingSong != null && existingSong.AnalyzedFileHash == fileHash)
                {
                    _logger.Info($"Same hash already in DB for file '{songFile.FullName}'");
                    parsedSongIds.Add(existingSong.Id);
                    return;
                }
            }

            var parsedSong = await _songParserService.ParseSongAsync(collectionId, songFile, fileHash, cancellationToken);

            if (parsedSong != null)
            {
                if (existingSong != null)
                {
                    dbContext.Songs.Remove(existingSong);
                }

                var addedSongEntity = await dbContext.Songs.AddAsync(parsedSong, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                parsedSongIds.Add(addedSongEntity.Entity.Id);

                _logger.Info($"Song file '{songFile.FullName}' metadata stored in DB");
            }
        }
    }
}
