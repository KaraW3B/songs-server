using KaraWeb.Shared.Models.Songs.Medleys;
using System;

namespace KaraWeb.Core.Persistence.Models.Songs
{
    public class SongMedley : ISongMedley
    {
        public TimeSpan MedleyStart { get; set; }

        public TimeSpan MedleyEnd { get; set; }

        public SongMedleyDto ToDto()
        {
            return new SongMedleyDto
            {
                MedleyStart = MedleyStart,
                MedleyEnd = MedleyEnd
            };
        }
    }
}
