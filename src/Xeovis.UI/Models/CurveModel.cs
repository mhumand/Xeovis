using System;
using System.Collections.Generic;
using System.Drawing;
using Xeovis.UI.Models.Enums;

namespace Xeovis.UI.Models
{
    public class CurveModel
    {
        public string Name { get; set; }
        public List<double> Xvals { get; set; }
        public List<double> Yvals { get; set; }
        public Guid Id { get; set; }
        public bool IsHidden { get; set; }
        public int TransparencyIn100 { get; set; }
        public CurveType CurveType { get; set; }

        // Line Properties
        public Color LineColor { get; set; }
        public int LineWidth { get; set; }
        public LineType LineType { get; set; }
        public bool IsLineVisible { get; set; }
        public bool IsLineAntiAlis {  get; set; }

        // Marker Properties
        public Color MarkerColor { get; set; }
        public int MarkerSize { get; set; }
        public MarkerType MarkerType { get; set; }
        public bool IsMarkerVisible { get; set; }

        public int DataCount => Math.Min(Xvals.Count, Yvals.Count);

        public CurveModel()
        {
            Name = "CurveModelTitle";
            Xvals = new List<double>();
            Yvals = new List<double>();

            Id = Guid.NewGuid();
            MarkerType = MarkerType.FilledCircle;
            LineType = LineType.Solid;
            LineColor = Color.Black;
            MarkerColor = Color.Black;
            MarkerSize = 6;
            LineWidth = 2;
            IsLineVisible = true;
            IsMarkerVisible = true;
            IsLineAntiAlis = true;
            TransparencyIn100 = 0;
            CurveType = CurveType.Scatter;
        }

        public int GetPointIndex(double x, double y)
        {
            var indX = Xvals.IndexOf(x);
            var indY = Yvals.IndexOf(y);

            if (indX == indY)
                return indX;
            else
                return -1;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
