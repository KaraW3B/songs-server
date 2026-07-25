namespace KaraW3B.Server.Songs.Models.Songs.Files
{
    /// <summary>
    ///     A song file with info
    /// </summary>
    public sealed class SongFileInfo
    {
        /// <summary>
        ///     The path to the file on the server
        /// </summary>
        /// <remarks>must be a relative path</remarks>
        public string FilePath { get; set; }

        /// <summary>
        ///     FFProbe info if available
        /// </summary>
        public FFProbeInfo FFProbeInfo { get; set; }
    }
}
