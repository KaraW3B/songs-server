using KaraWeb.Core.Models.Songs;
using KaraWeb.Core.Models.Songs.Notes;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using KaraWeb.Core.Helpers;
using log4net;

namespace KaraWeb.Core.Services.SongParser
{
    public sealed class SongParserService : ISongParserService
    {
        private const char ListSplitter = ',';
        private const string EofMarker = "E";

        private static readonly Regex HeaderRegex =
            new("^#(?<headerName>[A-Z0-9]+): *(?<headerValue>.+) *$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);

        private static readonly Regex PlayerRegex =
            new("^(DUETSINGER)?P(?<playerNumber>[1-9])$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);

        private static readonly Regex NoteRegex =
            new(@"^(?<noteType>[:*RGF-]) (?<startBeat>\d+)( (?<duration>\d+) (?<pitch>-?\d+) (?<text>.*))?$",
                RegexOptions.Compiled | RegexOptions.Singleline);

        private readonly ILog _logger = LogManager.GetLogger(nameof(SongParserService));

        public async Task<Song> ParseSongAsync(Guid collectionId, FileInfo songFile, string fileHash,
            CancellationToken cancellationToken)
        {
            if (!songFile.Exists)
            {
                _logger.Error($"The song file '{songFile.FullName}' was not found");
                return null;
            }

            _logger.Info($"Start parsing song file '{songFile.FullName}'");
            var timeWatch = new Stopwatch();
            timeWatch.Start();


            // TODO: Check encoding
            var encoding = Encoding.UTF8;

            try
            {
                var fileLines = await File.ReadAllLinesAsync(songFile.FullName, cancellationToken);

                var song = new Song
                {
                    CollectionId = collectionId,
                    SongFilePath = songFile.FullName,
                    AnalyzedFileHash = fileHash, 
                    HasEofMarker = fileLines.Any(IsEofMarker)
                };

                // Parsing headers
                await Parallel.ForEachAsync(fileLines, cancellationToken, (l, c) => ParseHeaders(song, l, c));
                await ParseNotes(song, fileLines, cancellationToken);

                await SongHelper.CheckHeadersErrorsAsync(song, cancellationToken);
                await SongHelper.CheckNotesErrorsAsync(song, cancellationToken);

                timeWatch.Stop();
                _logger.Info($"Song file '{songFile.FullName}' successfully parsed in {timeWatch.Elapsed}");

                return song;
            }
            catch (Exception e)
            {
                timeWatch.Stop();
                _logger.Error($"There was an error when parsing song file '{songFile.FullName}': {e}");
                return null;
            }
        }

        private static bool IsEofMarker(string line)
        {
            return line.Trim().Equals(EofMarker, StringComparison.InvariantCultureIgnoreCase);
        }

        #region Headers

        private ValueTask ParseHeaders(Song song, string fileLine, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var headerLineMatch = HeaderRegex.Match(fileLine);
            if (!headerLineMatch.Success)
            {
                return ValueTask.CompletedTask;
            }

            var headerName = headerLineMatch.Groups["headerName"].Value.ToUpperInvariant();
            var headerValue = headerLineMatch.Groups["headerValue"].Value;
            
            cancellationToken.ThrowIfCancellationRequested();
            if (HandleCoreHeaders(headerName, headerValue, song))
            {
                return ValueTask.CompletedTask;
            }


            cancellationToken.ThrowIfCancellationRequested();
            if (HandleExtraHeaders(headerName, headerValue, song))
            {
                return ValueTask.CompletedTask;
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (HandlePlayerHeaders(headerName, headerValue, song))
            {
                return ValueTask.CompletedTask;
            }

            song.NotManagedHeaders.Add($"{headerName}={headerValue}");
            return ValueTask.CompletedTask;
        }

        private bool HandleCoreHeaders(string headerName, string headerValue, Song song)
        {
            switch (headerName)
            {
                case "VERSION":
                    song.Version = headerValue;
                    return true;

                case "BPM":
                    if (double.TryParse(headerValue, CultureInfo.InvariantCulture, out var bpm))
                    {
                        song.Bpm = bpm;
                    }
                    return true;

                // For legacy support
                case "MP3":
                case "AUDIO":
                    song.Audio = headerValue;
                    return true;

                case "TITLE":
                    song.Title = headerValue;
                    return true;

                case "ARTIST":
                    song.Artist = headerValue;
                    return true;

                case "GAP":
                    if (int.TryParse(headerValue, CultureInfo.InvariantCulture, out var gap))
                    {
                        song.Gap = gap;
                    }
                    return true;

                case "START":
                    if (int.TryParse(headerValue, CultureInfo.InvariantCulture, out var start))
                    {
                        song.Start = start;
                    }
                    return true;

                case "END":
                    if (int.TryParse(headerValue, CultureInfo.InvariantCulture, out var end))
                    {
                        song.End = end;
                    }
                    return true;

                default:
                    return false;
            }
        }

        private bool HandleExtraHeaders(string headerName, string headerValue, Song song)
        {
            switch (headerName)
            {
                case "COVER":
                    song.Cover = headerValue;
                    return true;

                case "BACKGROUND":
                    song.Background = headerValue;
                    return true;

                case "VIDEO":
                    song.Video = headerValue;
                    return true;

                case "VOCALS":
                    song.Vocals = headerValue;
                    return true;

                case "INSTRUMENTAL":
                    song.Instrumental = headerValue;
                    return true;

                case "VIDEOGAP":
                    if (int.TryParse(headerValue, CultureInfo.InvariantCulture, out var videoGap))
                    {
                        song.VideoGap = videoGap;
                    }
                    return true;

                case "PREVIEWSTART":
                    if (int.TryParse(headerValue, CultureInfo.InvariantCulture, out var previewStart))
                    {
                        song.PreviewStart = previewStart;
                    }
                    return true;

                case "MEDLEYSTARTBEAT":
                case "MEDLEYSTART":
                    if (int.TryParse(headerValue, CultureInfo.InvariantCulture, out var medleyStart))
                    {
                        song.MedleyStart = medleyStart;
                    }
                    return true;

                case "MEDLEYENDBEAT":
                case "MEDLEYEND":
                    if (int.TryParse(headerValue, CultureInfo.InvariantCulture, out var medleyEnd))
                    {
                        song.MedleyEnd = medleyEnd;
                    }
                    return true;

                case "YEAR":
                    if (int.TryParse(headerValue, CultureInfo.InvariantCulture, out var year))
                    {
                        song.Year = year;
                    }
                    return true;

                case "GENRE":
                    song.Genres.AddRange(headerValue.Split(new[]{ ListSplitter }, StringSplitOptions.RemoveEmptyEntries).Select(v => v.Trim()));
                    return true;

                case "LANGUAGE":
                    song.Languages.AddRange(headerValue.Split(new[]{ ListSplitter }, StringSplitOptions.RemoveEmptyEntries).Select(v => v.Trim()));
                    return true;

                case "EDITION":
                    song.Editions.AddRange(headerValue.Split(new[]{ ListSplitter }, StringSplitOptions.RemoveEmptyEntries).Select(v => v.Trim()));
                    return true;

                case "TAGS":
                    song.Tags.AddRange(headerValue.Split(new[]{ ListSplitter }, StringSplitOptions.RemoveEmptyEntries).Select(v => v.Trim()));
                    return true;

                case "AUTHOR":
                case "CREATOR":
                    song.Creator = headerValue;
                    return true;

                case "PROVIDEDBY":
                    song.ProvidedBy = headerValue;
                    return true;

                case "COMMENT":
                    song.Comment = headerValue;
                    return true;

                case "AUDIOURL":
                    song.AudioUrl = headerValue;
                    return true;

                case "VIDEOURL":
                    song.VideoUrl = headerValue;
                    return true;

                case "COVERURL":
                    song.CoverUrl = headerValue;
                    return true;

                case "BACKGROUNDURL":
                    song.BackgroundUrl = headerValue;
                    return true;

                case "RENDITION":
                    song.Rendition = headerValue;
                    return true;

                default:
                    return false;
            }
        }

        private bool HandlePlayerHeaders(string headerName, string headerValue, Song song)
        {
            var playerHeaderMatch = PlayerRegex.Match(headerName);
            if (!playerHeaderMatch.Success)
            {
                return false;
            }

            if (!int.TryParse(playerHeaderMatch.Groups["playerNumber"].Value, out var playerNumber))
            {
                return false;
            }

            song.Players.Add(new SongPlayer { SongId = song.Id, Number = playerNumber, Name = headerValue });
            return true;
        }

        #endregion

        #region Notes

        private Task ParseNotes(Song song, string[] fileLines, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                song.Notes.Clear();
                var currentPlayer = 1;
                foreach (var fileLine in fileLines)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (IsEofMarker(fileLine))
                    {
                        break;
                    }

                    var noteMatch = NoteRegex.Match(fileLine);
                    if (!noteMatch.Success)
                    {
                        var playerMatch = PlayerRegex.Match(fileLine);
                        if (playerMatch.Success && int.TryParse(playerMatch.Groups["playerNumber"].Value, CultureInfo.InvariantCulture,
                                out var playerNumber))
                        {
                            currentPlayer = playerNumber;
                        }
                        continue;
                    }

                    var note = new SongNote
                    {
                        PlayerNumber = currentPlayer,
                        SongId = song.Id,
                        Type = ParseNoteType(noteMatch.Groups["noteType"].Value),
                        StartBeat = int.Parse(noteMatch.Groups["startBeat"].Value, CultureInfo.InvariantCulture)
                    };

                    if (note.Type != NoteType.Eol)
                    {
                        note.Duration = int.TryParse(noteMatch.Groups["duration"].Value, CultureInfo.InvariantCulture,
                            out var duration)
                            ? duration
                            : null;
                        note.Pitch = int.TryParse(noteMatch.Groups["pitch"].Value, CultureInfo.InvariantCulture,
                            out var pitch)
                            ? pitch
                            : null;
                        note.Text = noteMatch.Groups["text"].Value;
                    }

                    song.Notes.Add(note);
                }
            }, cancellationToken);
        }

        private static NoteType ParseNoteType(string noteType)
        {
            return noteType.ToUpperInvariant() switch
            {
                "-" => NoteType.Eol,
                ":" => NoteType.Regular,
                "*" => NoteType.Golden,
                "R" => NoteType.Rap,
                "G" => NoteType.GoldenRap,
                "F" => NoteType.Freestyle,
                _ => NoteType.Unknow
            };
        }

        #endregion
    }
}