using System;

namespace DiveLogApplication.Models
{
    public class DiveEntry
    {
        public DiveEntry()
        {

        }

        public string Location { get; set; }
        public string DiveSite { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Duration { get; set; }
        public double MaxDepth { get; set; }
        public double AverageDepth { get; set; }

        public DiveEntry Clone()
        {
            DiveEntry newDiveEntry = (DiveEntry)MemberwiseClone();
            return newDiveEntry;

        }

    }
}
