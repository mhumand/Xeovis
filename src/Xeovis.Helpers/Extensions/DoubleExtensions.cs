namespace Xeovis.Base
{
    public static class DoubleExtensions
    {
        public static double Round(this double x, int integer)
        {
            return Math.Round(x, integer);
        }

        public static double GetDoubleValueFromText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return double.NaN;

            var isNumeric = double.TryParse(text, out double dval);
            return isNumeric ? dval : double.NaN;
        }
    }
}
