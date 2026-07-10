using System.IO;
using System.Threading;
using System.Threading.Tasks;
using KaraW3B.Server.Core.Persistence.Models.Songs;

namespace KaraW3B.Server.Core.Services.SongFileInterpreter
{
    public interface ISongFileInterpreterService
    {
        Task<bool> ParseSongAndCheckAsync(FileInfo songFile, Song songToUpdate,
            CancellationToken cancellationToken);

        Task WriteSongFile(Song songToWrite, string filePath, bool overwrite, CancellationToken cancellationToken);
    }
}