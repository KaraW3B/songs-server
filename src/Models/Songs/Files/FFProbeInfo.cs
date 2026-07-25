namespace KaraW3B.Server.Songs.Models.Songs.Files
{
    /// <summary>
    ///     Information get from FFprobe of the song file
    /// </summary>
    public sealed class FFProbeInfo
    {
        /// <summary>
        ///     The encoded_by metadata if available
        /// </summary>
        public string EncodedBy { get; set; }

        /// <summary>
        ///     The format of the file
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        ///     The codec used by the file
        /// </summary>
        public string Codec { get; set; }

        public FFProbeInfo Clone()
        {
            return new FFProbeInfo
            {
                Codec = Codec,
                Format = Format,
                EncodedBy = EncodedBy
            };
        }
    }
}
