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
        public Curve Curve;
        public double X;
        public double Y;

        public ChartPointSelectedEventArgs(Curve curve, int index)
        {
            Curve = curve;
            X = Curve.Xvals[index];
            Y = Curve.Yvals[index];
        }
    }
}
