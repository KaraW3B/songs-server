using System;

namespace KaraWeb.Shared.Helpers
{
    internal static class TimesHelper
    {
        public static TimeSpan? GetTimeFromBeat(decimal bpm, int beat, TimeSpan? gap)
        {
            if (!SongValidationHelper.IsBpmValid(bpm))
            {
                return null;
            }

            var timeValue = (double)(beat / bpm * 60);
            var time = TimeSpan.FromSeconds(timeValue);
            if (gap.HasValue)
            {
                time += gap.Value;
            }

            return time;
        }
    }
}