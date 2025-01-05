using System;

namespace DiveLogApplication.Models
{
    public class DiveLicense
    {
        public string LicenseLevel { get; set; }

        public string LicenseProvider { get; set; }

        public string DiveCentre { get; set; }

        public string Location { get; set; }

        public DateTime IssuedDate { get; set; }

        public DateTime ObtainedDuration { get; set; }

        public string Id { get; set; }

        public Guid UniqueId { get; set; }

    }
}
