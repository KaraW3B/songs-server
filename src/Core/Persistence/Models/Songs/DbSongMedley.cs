using System;
using KaraW3B.SDK.Helpers.Interfaces;
using KaraW3B.Server.Songs.Models.Songs;

namespace KaraW3B.Server.Songs.Core.Persistence.Models.Songs
{
    public sealed class DbSongMedley : ISongMedley
    {
        public TimeSpan MedleyStart { get; set; }

        public TimeSpan MedleyEnd { get; set; }

        public SongMedley ToSongMedley()
        {
            return new SongMedley
            {
                MedleyStart = MedleyStart,
                MedleyEnd = MedleyEnd
            };
        }
    }
}