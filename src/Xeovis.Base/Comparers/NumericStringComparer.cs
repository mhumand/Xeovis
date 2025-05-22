using System.Globalization;

namespace Xeovis.Base.Comparers
{
    public class NumericStringComparer : IComparer<string>
    {
        public static bool IsNumeric(string value)
        {
            return double.TryParse(value, out _);
        }

        public int Compare(string s1, string s2)
        {
            const int S1GreaterThanS2 = 1;
            const int S2GreaterThanS1 = -1;

            var IsNumeric1 = IsNumeric(s1);
            var IsNumeric2 = IsNumeric(s2);

            if (IsNumeric1 && IsNumeric2)
            {
                var i1 = Convert.ToDouble(s1);
                var i2 = Convert.ToDouble(s2);
                if (i1 > i2) return S1GreaterThanS2;
                if (i1 < i2) return S2GreaterThanS1;
                return 0;
            }

            if (IsNumeric1) return S2GreaterThanS1;
            if (IsNumeric2) return S1GreaterThanS2;
            return string.Compare(s1, s2, true, CultureInfo.InvariantCulture);
        }
    }
}
