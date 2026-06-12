using System;
using KaraWeb.Shared.Helpers;

namespace KaraWeb.Core.Helper
{
    internal static class TimesHelper
    {
        public static TimeSpan? GetTimeFromBeat(decimal bpm, int beat, TimeSpan? gap)
        {
            if (!SongValidationHelper.IsBpmValid(bpm))
            {
                return null;
            }

            switch (beat)
            {
                case < 0:
                    return null;
                case 0:
                    return gap ?? TimeSpan.Zero;
            }

            var timeInSong = TimeSpan.FromMinutes((double)(beat / bpm));
            if (gap.HasValue)
            {
                timeInSong += gap.Value;
            }

            return timeInSong;
        }

        public static int? GetBeatFromTime(decimal bpm, TimeSpan time, TimeSpan? gap)
        {
            if (!SongValidationHelper.IsBpmValid(bpm))
            {
                return null;
            }

            var timeInSong = time;
            if (gap.HasValue)
            {
                var gapFactor = gap.Value < TimeSpan.Zero ? -2 : 1;
                timeInSong -= gap.Value * gapFactor;
            }

            if (timeInSong < TimeSpan.Zero)
            {
                return null;
            }

            if (timeInSong == TimeSpan.Zero)
            {
                return 0;
            }

            return (int)Math.Floor((decimal)timeInSong.TotalMinutes * bpm);
        }
    }
}