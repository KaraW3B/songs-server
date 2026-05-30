using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KaraWeb.Core.Models.Songs;

namespace KaraWeb.Core.Services.SongParser
{
    public sealed class SongParserService : ISongParserService
    {
        public async Task<SongParsingResult> ParseSongAsync(int collectionId, FileInfo songFile, CancellationToken cancellationToken)
        {
            if (!songFile.Exists)
            {
                return SongParsingResult.Error($"The song file '{songFile.FullName}' was not found");
            }

            // TODO: Check encoding
            var fileLines = await File.ReadAllLinesAsync(songFile.FullName, cancellationToken);

            var song = new Song
            {
                CollectionId = collectionId, 
                Title = fileLines.Single(l => l.Trim().StartsWith("#TITLE", StringComparison.InvariantCultureIgnoreCase)).Replace("#TITLE", string.Empty).Trim()[1..].Trim(),
                Artist = fileLines.Single(l => l.Trim().StartsWith("#ARTIST", StringComparison.InvariantCultureIgnoreCase)).Replace("#ARTIST", string.Empty).Trim()[1..].Trim(),
                Bpm = int.Parse(fileLines.Single(l => l.Trim().StartsWith("#BPM", StringComparison.InvariantCultureIgnoreCase)).Replace("#BPM", string.Empty).Trim()[1..].Trim()),
                Audio = fileLines.Single(l => l.Trim().StartsWith("#MP3", StringComparison.InvariantCultureIgnoreCase) || l.Trim().StartsWith("#AUDIO", StringComparison.InvariantCultureIgnoreCase)).Replace("#MP3", string.Empty).Replace("#AUDIO", string.Empty).Trim()[1..].Trim(),
            };

            return SongParsingResult.Success(song);
        }
    }
}
