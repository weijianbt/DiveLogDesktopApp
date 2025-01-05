using System;

namespace DiveLogApplication.Utilities
{
    public static class GenerateRandomID
    {
        static GenerateRandomID()
        {

        }

        public static Guid Generate()
        {
            return Guid.NewGuid();
        }
    }
}
