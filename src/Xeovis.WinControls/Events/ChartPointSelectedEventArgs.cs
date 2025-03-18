using System;

namespace Xeovis.WinControls
{
    public class ChartPointSelectedEventArgs : EventArgs
    {
        public CurveModel Curve;
        public double X;
        public double Y;

        public ChartPointSelectedEventArgs(CurveModel curve, int index)
        {
            Curve = curve;
            X = index >= 0 ? Curve.Xvals[index] : double.NaN;
            Y = index >= 0 ? Curve.Yvals[index] : double.NaN;
        }
    }
}
