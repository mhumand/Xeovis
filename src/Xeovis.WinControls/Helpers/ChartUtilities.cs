using ZedGraph;

namespace Xeovis.WinControls
{
    public static class ChartUtilities
    {
        public static Color GetColorInGradient(int index, int count)
        {
            var low = Color.Blue;
            var high = Color.Red;

            var R = (high.R - low.R) * index / count + low.R;
            var G = (high.G - low.G) * index / count + low.G;
            var B = (high.B - low.B) * index / count + low.B;

            return Color.FromArgb(R, G, B);
        }

        public static Color GetColor(int index)
        {
            var colors = new List<Color>()
            {
                Color.Blue,
                Color.Orange,
                Color.Green,
                Color.Red,
                Color.Brown,
                Color.Magenta,
                Color.DeepSkyBlue,
                Color.Black,
                Color.LawnGreen,
                Color.Olive,
            };

            if (index >= colors.Count)
                return Color.Transparent;

            return colors[index];
        }

        public static SymbolType GetMarkerType(int index)
        {
            var symbols = new List<SymbolType>()
            {
                SymbolType.Square,
                SymbolType.Triangle,
                SymbolType.Circle,
                SymbolType.TriangleDown,
                SymbolType.Plus,
                SymbolType.Star,
            };

            if (index >= symbols.Count)
                return SymbolType.Default;

            return symbols[index];
        }
    }
}
