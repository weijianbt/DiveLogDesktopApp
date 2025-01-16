using DiveLogApplication.Core;
using System;

namespace DiveLogApplication.Models
{
    public class DiveEntry : ViewModel
    {
        public DiveEntry()
        {

        }

        public string DiveSite { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Duration { get; set; }
        public double MaxDepth { get; set; }
        public double AverageDepth { get; set; }

    }
}
