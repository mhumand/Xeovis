using System.Collections;
using System.Globalization;

namespace Xeovis.Base.Comparers
{
    public class NumericDictionaryComparer : IComparer
    {
        private string _key;
        public NumericDictionaryComparer(string key)
        {
            _key = key;
        }

        public int Compare(object? obj1, object? obj2)
        {
            var dic1 = (Dictionary<string, object>)obj1;
            var dic2 = (Dictionary<string, object>)obj2;
            var s1 = dic1[_key].ToString();
            var s2 = dic2[_key].ToString();

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

        private static bool IsNumeric(string value)
        {
            return double.TryParse(value, out _);
        }
    }
}
