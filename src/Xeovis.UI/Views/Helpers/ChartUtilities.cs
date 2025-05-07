using System;
using System.Collections.Generic;
using System.Drawing;
using Xeovis.Presentation;

namespace Xeovis.UI
{
    public static class ChartUtilities
    {
        public static List<Color> ColorPalette_ColorblindFriendly = new List<Color>()
        {
            ColorTranslator.FromHtml("#000000"),
            ColorTranslator.FromHtml("#E69F00"),
            ColorTranslator.FromHtml("#56B4E9"),
            ColorTranslator.FromHtml("#009E73"),
            ColorTranslator.FromHtml("#F0E442"),
            ColorTranslator.FromHtml("#0072B2"),
            ColorTranslator.FromHtml("#D55E00"),
            ColorTranslator.FromHtml("#CC79A7"),
        };

        public static List<Color> ColorPalette_18 = new List<Color>()
        {
            ColorTranslator.FromHtml("#1F77B4"),
            ColorTranslator.FromHtml("#FF7F0E"),
            ColorTranslator.FromHtml("#2CA02C"),
            ColorTranslator.FromHtml("#D62728"),
            ColorTranslator.FromHtml("#9467BD"),
            ColorTranslator.FromHtml("#8C564B"),
            ColorTranslator.FromHtml("#E377C2"),
            ColorTranslator.FromHtml("#BCBD22"),
            ColorTranslator.FromHtml("#17BECF"),
            ColorTranslator.FromHtml("#AEC7E8"),
            ColorTranslator.FromHtml("#FFBB78"),
            ColorTranslator.FromHtml("#98DF8A"),
            ColorTranslator.FromHtml("#FF9896"),
            ColorTranslator.FromHtml("#C5B0D5"),
            ColorTranslator.FromHtml("#C49C94"),
            ColorTranslator.FromHtml("#F7B6D2"),
            ColorTranslator.FromHtml("#DBDB8D"),
            ColorTranslator.FromHtml("#9EDAE5"),
        };

        public static List<MarkerType> MarkerTypes = new List<MarkerType>()
        {
            MarkerType.FilledCircle,
            MarkerType.FilledSquare,
            MarkerType.FilledTriangleDown,
            MarkerType.FilledTriangleUp,
            MarkerType.FilledDiamond
        };

        public static Color GetColorInGradient(Color lowColor, Color highColor, int index, int count)
        {
            var R = (highColor.R - lowColor.R) * index / count + lowColor.R;
            var G = (highColor.G - lowColor.G) * index / count + lowColor.G;
            var B = (highColor.B - lowColor.B) * index / count + lowColor.B;

            return Color.FromArgb(R, G, B);
        }

        public static Color GetColorInGradient(int index, int count)
        {
            var highColor = Color.Red;
            var lowColor = Color.Blue;

            return GetColorInGradient(lowColor, highColor, index, count);
        }

        public static Color GetColor_ColorblindFriendlyPalete(int index)
        {
            if (index < ColorPalette_ColorblindFriendly.Count)
                return ColorPalette_ColorblindFriendly[index];
            else
                return Color.Transparent;
        }

        public static MarkerType GetMarkerType(int index)
        {
            if (index < MarkerTypes.Count)
                return MarkerTypes[index];
            else
                return MarkerType.None;
        }

        public static void SetColorAndMarker(CurveModel curve, int index)
        {
            var pallete = ColorPalette_18;

            if (index >= MarkerTypes.Count * pallete.Count)
                throw new IndexOutOfRangeException();

            var markerIndex = index / pallete.Count;
            var colorIndex = index % pallete.Count;
            curve.MarkerColor = pallete[colorIndex];
            curve.MarkerType = MarkerTypes[markerIndex];
        }
    }
}
