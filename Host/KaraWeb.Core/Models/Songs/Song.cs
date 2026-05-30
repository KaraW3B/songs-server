using KaraWeb.Core.Models.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KaraWeb.Core.Models.Songs
{
    /// <summary>
    /// A song parsed from file
    /// </summary>
    [Table("Songs")]
    public sealed class Song
    {
        /// <summary>
        /// The song's ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// The collection song belongs to
        /// </summary>
        [ForeignKey(nameof(Collection))]
        public int CollectionId { get; set; }
        [JsonIgnore]
        public Collection Collection { get; set; }

        #region Core headers

        /// <summary>
        /// The UltraStar format version used
        /// </summary>
        [MaxLength(6)]
        public string Version { get; set; }

        /// <summary>
        /// The BPM of the song
        /// </summary>
        [Required]
        public int Bpm { get; set; }

        /// <summary>
        /// The song's title
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The song's artist
        /// </summary>
        [MaxLength(500)]
        [Required]
        public string Artist { get; set; }
        
        /// <summary>
        /// The audio file path
        /// </summary>
        /// <remarks>must be a relative path</remarks>
        [Required]
        public string Audio { get; set; }

        /// <summary>
        /// The GAP between start of the audio and first beat
        /// </summary>
        /// <remarks>milliseconds</remarks>
        public int Gap { get; set; }

        /// <summary>
        /// The GAP between start of the audio and first beat
        /// </summary>
        /// <remarks>milliseconds</remarks>
        public int Start { get; set; }

        /// <summary>
        /// The GAP between start of the audio and first beat
        /// </summary>
        /// <remarks>milliseconds</remarks>
        public int End { get; set; }

        /// <summary>
        /// Song's players defined from #P1 to #P9
        /// </summary>
        public List<SongPlayer> Players { get; set; }

        #endregion

        #region Extra headers

        /// <summary>
        /// The cover file path
        /// </summary>
        /// <remarks>must be a relative path</remarks>
        public string Cover { get; set; }

        /// <summary>
        /// The background file path
        /// </summary>
        /// <remarks>must be a relative path</remarks>
        public string Background { get; set; }

        /// <summary>
        /// The video file path
        /// </summary>
        /// <remarks>must be a relative path</remarks>
        public string Video { get; set; }

        /// <summary>
        /// The video delay relative to the audio file
        /// </summary>
        /// <remarks>milliseconds</remarks>
        public int VideoGap { get; set; }

        /// <summary>
        /// The vocals file path
        /// </summary>
        /// <remarks>must be a relative path</remarks>
        public string Vocals { get; set; }

        /// <summary>
        /// The instrumental file path
        /// </summary>
        /// <remarks>must be a relative path</remarks>
        public string Instrumental { get; set; }

        /// <summary>
        /// The offset of audio file to use for preview
        /// </summary>
        /// <remarks>milliseconds</remarks>
        public int PreviewStart { get; set; }

        /// <summary>
        /// The offset of audio file to use to start in a medley
        /// </summary>
        /// <remarks>milliseconds</remarks>
        public int MedleyStart { get; set; }

        /// <summary>
        /// The offset of audio file to use to finish in a medley
        /// </summary>
        /// <remarks>milliseconds</remarks>
        public int MedleyEnd { get; set; }

        /// <summary>
        /// The song's year
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Song's genres
        /// </summary>
        public List<string> Genres { get; set; }

        /// <summary>
        /// The song's language
        /// </summary>
        /// <remarks>Using ISO 639-2 english languages</remarks>
        [MaxLength(30)]
        public string Language { get; set; }

        /// <summary>
        /// Song's editions
        /// </summary>
        public List<string> Editions { get; set; }

        /// <summary>
        /// Song's tags
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// The song's creator
        /// </summary>
        [MaxLength(500)]
        public string Creator { get; set; }

        /// <summary>
        /// The source of the song
        /// </summary>
        /// <remarks>valid http(s) URL according to RFC 1738</remarks>
        public string ProvidedBy { get; set; }

        /// <summary>
        /// Comment about the song
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// The source of the audio used
        /// </summary>
        /// <remarks>valid http(s) URL according to RFC 1738</remarks>
        public string AudioUrl { get; set; }

        /// <summary>
        /// The source of the video used
        /// </summary>
        /// <remarks>valid http(s) URL according to RFC 1738</remarks>
        public string VideoUrl { get; set; }

        /// <summary>
        /// The source of the cover used
        /// </summary>
        /// <remarks>valid http(s) URL according to RFC 1738</remarks>
        public string CoverUrl { get; set; }

        /// <summary>
        /// The source of the background used
        /// </summary>
        /// <remarks>valid http(s) URL according to RFC 1738</remarks>
        public string BackgroundUrl { get; set; }

        /// <summary>
        /// Specific recording of the song
        /// </summary>
        [MaxLength(300)]
        public string Rendition { get; set; }

        #endregion

        #region Parsing

        /// <summary>
        /// The set of parsing errors
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// The set of parsing warnings
        /// </summary>
        public List<string> Warnings { get; set; }

        #endregion
    }
}
