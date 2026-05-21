using System;
using System.Collections.Generic;
using System.Drawing;
using Serilog;
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

        public static List<Color> ColorPalette = new List<Color>()
        {
            ColorTranslator.FromHtml("#4E79A7"), // Blue
            ColorTranslator.FromHtml("#F28E2B"), // Orange
            ColorTranslator.FromHtml("#E15759"), // Red
            ColorTranslator.FromHtml("#76B7B2"), // Teal
            ColorTranslator.FromHtml("#59A14F"), // Green
            ColorTranslator.FromHtml("#EDC948"), // Yellow
            ColorTranslator.FromHtml("#B07AA1"), // Purple
            ColorTranslator.FromHtml("#FF9DA7"), // Pink
            ColorTranslator.FromHtml("#9C755F"), // Brown
            ColorTranslator.FromHtml("#BAB0AC"), // Gray

            // --- Tol Bright & Vibrant extensions ---
            ColorTranslator.FromHtml("#4477AA"), // Deep Blue
            ColorTranslator.FromHtml("#66CCEE"), // Sky Blue
            ColorTranslator.FromHtml("#228833"), // Dark Green
            ColorTranslator.FromHtml("#CCBB44"), // Mustard
            ColorTranslator.FromHtml("#EE6677"), // Coral Red
            ColorTranslator.FromHtml("#AA3377"), // Magenta Purple
            ColorTranslator.FromHtml("#BBBBBB"), // Mid Grey
            ColorTranslator.FromHtml("#000000"), // Black

            // --- D3 Category20b-like hues ---
            ColorTranslator.FromHtml("#393B79"), // Indigo
            ColorTranslator.FromHtml("#637939"), // Olive Green
            ColorTranslator.FromHtml("#8C6D31"), // Warm Brown
            ColorTranslator.FromHtml("#843C39"), // Dark Red
            ColorTranslator.FromHtml("#7B4173"), // Deep Violet
            ColorTranslator.FromHtml("#5254A3"), // Blue Violet
            ColorTranslator.FromHtml("#8CA252"), // Olive
            ColorTranslator.FromHtml("#BD9E39"), // Golden Brown
            ColorTranslator.FromHtml("#AD494A"), // Muted Crimson
            ColorTranslator.FromHtml("#A55194"), // Magenta Violet

            // --- Light tint variants for contrast ---
            ColorTranslator.FromHtml("#9C9EDE"), // Light Periwinkle
            ColorTranslator.FromHtml("#B5CF6B"), // Light Lime
            ColorTranslator.FromHtml("#E7BA52"), // Warm Gold
            ColorTranslator.FromHtml("#E7969C"), // Light Rose
            ColorTranslator.FromHtml("#DE9ED6"), // Light Pink Purple
        };

        public static List<MarkerType> MarkerTypes = new List<MarkerType>()
        {
            MarkerType.FilledCircle,
            MarkerType.FilledSquare,
            MarkerType.FilledTriangleDown,
            MarkerType.FilledTriangleUp,
            MarkerType.FilledDiamond,
            MarkerType.Asterisk,
            MarkerType.Cross
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
            return index < MarkerTypes.Count ? MarkerTypes[index] : MarkerType.None;
        }

        public static void SetColorAndMarker(CurveModel curve, int index)
        {
            var pallete = ColorPalette;

            if (index >= MarkerTypes.Count * pallete.Count)
            {
                Log.Warning($"Index {index} is out of range for available marker+color combinations");
                return;
            }

            var markerIndex = index / pallete.Count;
            var colorIndex = index % pallete.Count;
            curve.MarkerColor = pallete[colorIndex];
            curve.MarkerType = MarkerTypes[markerIndex];
        }

        public static bool IsInvisible(this Color color)
        {
            return color.IsEmpty || color == Color.Transparent || color == Color.White;
        }
    }
}
