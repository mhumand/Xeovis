using System.Drawing.Drawing2D;
using ZedGraph;

namespace Xeovis.WinControls
{
    public class CurveModel
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
        public int Transparency { get; set; }
        public CurveType CurveType { get; set; }

        public CurveModel()
        {
            Xvals = new List<double>();
            Yvals = new List<double>();

            ID = Guid.NewGuid();
            MarkerType = SymbolType.Circle;
            LineColor = Color.Black;
            MarkerColor = Color.Black;
            MarkerSize = 6;
            LineWidth = 2;
            IsLineVisible = true;
            IsMarkerVisible = true;
            Transparency = 0;
            CurveType = CurveType.Normal;
        }
    }
}
