namespace Xeovis.Base
{
    public static class DoubleExtensions
    {
        public static double Round(this double x, int integer)
        {
            return integer < 0 ? x : Math.Round(x, integer);
        }

        public static double GetDoubleValueFromText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return double.NaN;

            var isNumeric = double.TryParse(text, out double dval);
            return isNumeric ? dval : double.NaN;
        }
    }
}
