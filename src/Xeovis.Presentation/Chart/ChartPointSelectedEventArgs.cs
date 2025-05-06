namespace Xeovis.Presentation
{
    public class ChartPointSelectedEventArgs : EventArgs
    {
        public double X;
        public double Y;
        public Guid CurveId { get; set; }
        public int Index { get; set; }

        public bool HasValidPoint => !double.IsNaN(X) && !double.IsInfinity(Y);
        public bool HasValidCurveId => CurveId != Guid.Empty;

        public ChartPointSelectedEventArgs(double x, double y, int index = -1, Guid curveId = new Guid())
        {
            X = x;
            Y = y;
            Index = index;
            CurveId = curveId;
        }
    }
}
