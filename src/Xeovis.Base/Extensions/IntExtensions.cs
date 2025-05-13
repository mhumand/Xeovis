namespace Xeovis.Base
{
    public static class IntExtensions
    {
        public static bool IsAlmostEqual(this int a, int b, int threshold)
        {
            return Math.Abs(a - b) <= threshold;
        }

        public static bool IsBetween(this int a, int lowerBound, int upperBound, bool inclusive = true)
        {
            if (inclusive)
                return a >= lowerBound && a <= upperBound;
            else
                return a < upperBound && a > lowerBound;
        }
    }
}
