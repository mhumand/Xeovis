using System;
using System.Collections.Generic;
using System.Drawing;
using ZedGraph;

namespace DatabaseReader.Helpers
{
    public static class ChartUtilities
    {
        public static Color GetNewColor(List<Color> otherColors)
        {
            var rnd = new Random();
            var rndR = rnd.Next(200);
            var rndG = rnd.Next(200);
            var rndB = rnd.Next(200);

            bool rotate = true;
            while (rotate)
            {
                bool foundAny = false;
                foreach (var color in otherColors)
                {
                    int r = color.R;
                    int g = color.G;
                    int b = color.B;

                    if (rndR.IsAlmostEqual(r, 20) || rndG.IsAlmostEqual(g, 20) || rndB.IsAlmostEqual(b, 20))
                    {
                        foundAny = true;
                        break;
                    }
                }

                if (foundAny)
                {
                    rndR = rnd.Next(200);
                    rndG = rnd.Next(200);
                    rndB = rnd.Next(200);
                }
                
                rotate = foundAny;
            }

            return Color.FromArgb(rndR, rndG, rndB);
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
