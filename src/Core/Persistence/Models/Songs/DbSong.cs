using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using KaraW3B.Interpreters.Interfaces;
using KaraW3B.Server.Songs.Core.Persistence.Models.Libraries;
using KaraW3B.Server.Songs.Models.Songs;
using KaraW3B.Server.Songs.Models.Songs.Alerts;
using KaraW3B.Server.Songs.Models.Songs.Files;
using Microsoft.EntityFrameworkCore;

namespace KaraW3B.Server.Songs.Core.Persistence.Models.Songs
{
    [Table("Songs")]
    [PrimaryKey(nameof(Id))]
    public class DbSong
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Library))]
        public Guid LibraryId { get; set; }

        public virtual DbLibrary Library { get; set; }

        #region Core headers

        public Version Version { get; set; }

        public decimal Bpm { get; set; }

        [MaxLength(1000)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Artist { get; set; }

        public DbSongFile Audio { get; set; }

        public TimeSpan? Gap { get; set; }

        public TimeSpan? Start { get; set; }

        public TimeSpan? End { get; set; }

        public virtual List<DbSongPlayer> Players { get; set; } = new();

        public Dictionary<int, string> GetPlayers()
        {
            return Players.ToDictionary(p => p.Number, p => p.Name);
        }

        #endregion

        #region Extra headers

        public string Cover { get; set; }

        public string Background { get; set; }

        public DbSongFile Video { get; set; }

        public TimeSpan? VideoGap { get; set; }

        public DbSongFile Vocals { get; set; }

        public DbSongFile Instrumental { get; set; }

        public TimeSpan? PreviewStart { get; set; }

        public virtual DbSongMedley Medley { get; set; }

        public ISongMedley GetMedley()
        {
            return Medley;
        }

        public int? Year { get; set; }

        public List<string> Genres { get; set; } = new();

        public List<string> Languages { get; set; } = new();

        public List<string> Editions { get; set; } = new();

        public List<string> Tags { get; set; } = new();

        public List<string> Creators { get; set; } = new();

        public string ProvidedBy { get; set; }

        public string Comment { get; set; }

        public string AudioUrl { get; set; }

        public string VideoUrl { get; set; }

        public string CoverUrl { get; set; }

        public string BackgroundUrl { get; set; }

        [MaxLength(300)]
        public string Rendition { get; set; }

        public List<string> NotManagedHeaders { get; set; } = new();

        #endregion

        #region Internal

        public virtual List<DbSongAlert> Alerts { get; set; } = new();

        public virtual List<DbSongNote> Notes { get; set; } = new();

        [Required]
        public string SongFilePath { get; set; }

        [NotMapped]
        public string SongDirectory => Path.GetDirectoryName(SongFilePath);

        [Required]
        public string AnalyzedFileHash { get; set; }

        [Required]
        public DateTime LastParseTime { get; set; }

        #endregion

        public string GetSongFilePath(FileType fileType)
        {
            var filePath = fileType switch
            {
                FileType.Audio => Audio?.FilePath,
                FileType.Cover => Cover,
                FileType.Background => Background,
                FileType.Video => Video?.FilePath,
                FileType.Vocals => Vocals?.FilePath,
                FileType.Instrumental => Instrumental?.FilePath,
                _ => null
            };

            return string.IsNullOrEmpty(filePath) ? null : Path.Combine(SongDirectory, filePath);
        }

        public bool SongFileExist(FileType fileType)
        {
            var songFilePath = GetSongFilePath(fileType);
            return !string.IsNullOrEmpty(songFilePath) && File.Exists(songFilePath);
        }

        public Song ToSong()
        {
            var songDto = new Song
            {
                Id = Id,
                Version = Version,
                Bpm = Bpm,
                Title = Title,
                Artist = Artist,
                Audio = Audio?.ToSongFileInfo(),
                Gap = Gap,
                Start = Start,
                End = End,
                Players = Players.Select(p => p.ToSongPlayer()).ToList(),
                Cover = Cover,
                Background = Background,
                Video = Video?.ToSongFileInfo(),
                Vocals = Vocals?.ToSongFileInfo(),
                Instrumental = Instrumental?.ToSongFileInfo(),
                AudioUrl = AudioUrl,
                VideoUrl = VideoUrl,
                CoverUrl = CoverUrl,
                BackgroundUrl = BackgroundUrl,
                VideoGap = VideoGap,
                PreviewStart = PreviewStart,
                Medley = Medley?.ToSongMedley(),
                Year = Year,
                Genres = Genres.ToList(),
                Languages = Languages.ToList(),
                Editions = Editions.ToList(),
                Tags = Tags.ToList(),
                Creators = Creators.ToList(),
                ProvidedBy = ProvidedBy,
                Comment = Comment,
                Rendition = Rendition,
                NotManagedHeaders = NotManagedHeaders.ToList(),
                LastParsedTime = LastParseTime,
                HasFatal = Alerts.Any(a => a.Level == AlertLevel.Fatal),
                HasErrors = Alerts.Any(a => a.Level == AlertLevel.Error),
                HasWarnings = Alerts.Any(a => a.Level == AlertLevel.Warning)
            };
            return songDto;
        }
    }
}