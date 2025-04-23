using System;
using System.Collections.Generic;
using System.Drawing;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;

namespace Xeovis.UI
{
    public static class ChartUtilities
    {
        public static List<Color> ColorPallete_ColorblindFriendly = new List<Color>()
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

            var R = (highColor.R - lowColor.R) * index / count + lowColor.R;
            var G = (highColor.G - lowColor.G) * index / count + lowColor.G;
            var B = (highColor.B - lowColor.B) * index / count + lowColor.B;

            return Color.FromArgb(R, G, B);
        }

        public static Color GetColor_ColorblindFriendlyPallete(int index)
        {
            if (index < ColorPallete_ColorblindFriendly.Count)
                return ColorPallete_ColorblindFriendly[index];
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
            if (index >= MarkerTypes.Count * ColorPallete_ColorblindFriendly.Count)
                throw new IndexOutOfRangeException();

            var markerIndex = index / ColorPallete_ColorblindFriendly.Count;
            var colorIndex = index % ColorPallete_ColorblindFriendly.Count;
            curve.MarkerColor = ColorPallete_ColorblindFriendly[colorIndex];
            curve.MarkerType = MarkerTypes[markerIndex];
        }
    }
}
