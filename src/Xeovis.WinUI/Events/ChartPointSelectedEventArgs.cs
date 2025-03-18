using DatabaseReader.Definitions.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseReader.Events
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
