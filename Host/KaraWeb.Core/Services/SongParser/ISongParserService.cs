using System.IO;
using System.Threading;
using System.Threading.Tasks;
using KaraWeb.Core.Models.Songs;

namespace KaraWeb.Core.Services.SongParser
{
    public interface ISongParserService
    {
        Task<SongParsingResult> ParseSongAsync(int collectionId, FileInfo songFile, CancellationToken cancellationToken);
    }
}
