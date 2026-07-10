using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using KaraW3B.SDK.Helpers;
using KaraW3B.SDK.Interpreters;
using KaraW3B.SDK.Models.Songs.Alerts;
using KaraW3B.Server.Core.Persistence.Models.Songs;
using log4net;

namespace KaraW3B.Server.Core.Services.SongFileInterpreter
{
    public sealed class SongFileInterpreterService : ISongFileInterpreterService
    {
        private readonly ILog _logger = LogManager.GetLogger(nameof(SongFileInterpreterService));

        public async Task<bool> ParseSongAndCheckAsync(FileInfo songFile, Song song,
            CancellationToken cancellationToken)
        {
            if (!songFile.Exists)
            {
                _logger.Error($"The song file '{songFile.FullName}' was not found");
                return false;
            }

            var songProxy = new InterpretableSongProxy(song);
            _logger.Info($"Start parsing song file '{songFile.FullName}'");
            
            var timeWatch = new Stopwatch();
            timeWatch.Start();

            await SongParser.ParseSongAsync(songFile, songProxy, cancellationToken);

            timeWatch.Stop();
            _logger.Info(
                $"Song file '{songFile.FullName}' successfully parsed in {timeWatch.Elapsed}");

            song.LastParseTime = DateTime.Now;

            await AnalyzeSong(song, cancellationToken);
            return true;
        }

        public async Task WriteSongFile(Song songToWrite, string filePath, bool overwrite, CancellationToken cancellationToken)
        {
            var songProxy = new InterpretableSongProxy(songToWrite);

            _logger.Info($"Start writing song file '{filePath}'");

            var timeWatch = new Stopwatch();
            timeWatch.Start();

            await SongWriter.WriteSongAsync(songProxy, filePath, overwrite, cancellationToken);

            timeWatch.Stop();
            _logger.Info(
                $"Song file '{filePath}' successfully wrote in {timeWatch.Elapsed}");
        }

        private static async Task AnalyzeSong(Song song, CancellationToken cancellationToken)
        {
            var analyzeResult = await SongValidationHelper.CheckFullSongErrorsAsync(song, song.Notes, cancellationToken);
            foreach (var infoError in analyzeResult.InfoErrors)
            {
                song.Alerts.Add(new SongAlert
                {
                    Type = AlertType.Info,
                    Level = infoError.IsWarning ? AlertLevel.Warning : AlertLevel.Error,
                    Message = infoError.Message
                });
            }

            foreach (var noteError in analyzeResult.NotesErrors)
            {
                song.Alerts.Add(new SongAlert
                {
                    Type = AlertType.Note,
                    Level = AlertLevel.Error,
                    Message = noteError.Message,
                    FileLine = noteError.FileLine
                });
            }
        }
    }
}