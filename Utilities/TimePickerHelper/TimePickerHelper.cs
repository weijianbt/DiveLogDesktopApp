using System.Collections.Generic;
using System.Linq;

namespace DiveLogApplication.Utilities.TimePickerHelper
{
    public static class TimePickerHelper
    {
        public static List<int> Hours { get; } = Enumerable.Range(1, 12).ToList();
        public static List<int> Minutes { get; } = Enumerable.Range(0, 60).ToList();
        public static List<DayOrNight> DayOrNights { get; } = new List<DayOrNight> {DayOrNight.AM, DayOrNight.PM};
    }
}
