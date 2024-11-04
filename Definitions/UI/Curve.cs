using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace DatabaseReader.Definitions.UI
{
    public class Curve
    {
        public string Name { get; set; }
        public List<double> Xvals { get; set; }
        public List<double> Yvals { get; set; }
        public Guid ID { get; set; }
        public Color LineColor { get; set; }
        public Color MarkerColor { get; set; }
        public int MarkerSize { get; set; }
        public int LineWidth { get; set; }
        public DashStyle LineType { get; set; }
        public SymbolType MarkerType { get; set; }
        public int DataCount => Math.Min(Xvals.Count, Yvals.Count);
        public bool IsLineVisible { get; set; }
        public bool IsMarkerVisible { get; set; }
        public bool IsHidden { get; set; }

        public Curve()
        {
            Xvals = new List<double>();
            Yvals = new List<double>();

            ID = Guid.NewGuid();
            MarkerType = SymbolType.Circle;
            LineColor = Color.Black;
            MarkerColor = Color.Black;
            MarkerSize = 8;
            LineWidth = 2;
            IsLineVisible = true;
            IsMarkerVisible = true;
        }
    }
}
