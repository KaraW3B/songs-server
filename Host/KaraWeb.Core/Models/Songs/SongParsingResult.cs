namespace KaraWeb.Core.Models.Songs
{
    /// <summary>
    /// The result of a song parsing
    /// </summary>
    public sealed class SongParsingResult
    {
        /// <summary>
        /// The parsed song
        /// </summary>
        public Song ParsedSong { get; private init; }

        /// <summary>
        /// The error message
        /// </summary>
        public string ErrorMessage { get; private init; }

        /// <summary>
        /// Indicates if the file was correctly parsed
        /// </summary>
        public bool IsSuccess => ParsedSong != null;

        private SongParsingResult(){ }

        public static SongParsingResult Success(Song song)
        {
            return new SongParsingResult { ParsedSong = song };
        }
        public static SongParsingResult Error(string error)
        {
            return new SongParsingResult { ErrorMessage = error };
        }
    }
}
