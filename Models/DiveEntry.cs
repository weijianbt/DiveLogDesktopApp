﻿using System;

namespace DiveLogApplication.Models
{
    public class DiveEntry
    {
        public DiveEntry()
        {

        }

        public uint DiveLogIndex { get; set; }
        public string Location { get; set; }
        public string DiveSite { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
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
