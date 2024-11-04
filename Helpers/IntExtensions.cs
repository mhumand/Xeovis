using System;

namespace DatabaseReader.Helpers
{
    public static class IntExtensions
    {
        public static bool IsAlmostEqual(this int a, int b, int threshold)
        {
            if (Math.Abs(a - b) <= threshold)
                return true;
            else
                return false;
        }
    }
}
