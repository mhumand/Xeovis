using DatabaseReader.Definitions.UI;
using DatabaseReader.Events;
using System;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace DatabaseReader.CustomControls
{
    public partial class ChartControl : UserControl
    {
        private ChartModel _chartModel;
        public ZedGraphControl Chart => zgChart;
        public EventHandler<ChartPointSelectedEventArgs> ChartPointSelected;
        public ChartControl()
        {
            InitializeComponent();
        }

        public void Plot(ChartModel chartmodel)
        {
            _chartModel = chartmodel;

            zgChart.GraphPane.CurveList.Clear();

            foreach (var curve in _chartModel.Curves)
            {
                if (curve.IsHidden || curve.DataCount == 0)
                    continue;

                var lineItem = zgChart.GraphPane.AddCurve(curve.Name, curve.Xvals.ToArray(), curve.Yvals.ToArray(), curve.LineColor);
                lineItem.Tag = curve;
                lineItem.Line.IsAntiAlias = true;
                lineItem.Line.IsVisible = curve.IsLineVisible;
                lineItem.Line.Color = curve.LineColor;
                lineItem.Line.Width = curve.LineWidth;
                lineItem.Line.Style = curve.LineType;

                lineItem.Symbol.IsVisible = curve.IsMarkerVisible;
                lineItem.Symbol.Size = curve.MarkerSize;
                lineItem.Symbol.Fill.Color = curve.MarkerColor;
                lineItem.Symbol.Fill.IsVisible = curve.IsMarkerVisible;
                lineItem.Symbol.Fill.Type = FillType.Solid;
                lineItem.Symbol.Border.Color = curve.MarkerColor;
                lineItem.Symbol.Type = curve.MarkerType;
            }

            // style the plot
            zgChart.GraphPane.Title.Text = _chartModel.Title;
            zgChart.GraphPane.Title.IsVisible = _chartModel.IsTitleVisible;
            zgChart.GraphPane.XAxis.Title.Text = _chartModel.XAxisLabel;
            zgChart.GraphPane.YAxis.Title.Text = _chartModel.YAxisLabel;

            zgChart.GraphPane.Legend.IsVisible = _chartModel.IsLegendVisible;
            zgChart.GraphPane.Legend.Border.IsVisible = false;

            // auto-axis and update the display
            zgChart.GraphPane.XAxis.ResetAutoScale(zgChart.GraphPane, CreateGraphics());
            zgChart.GraphPane.YAxis.ResetAutoScale(zgChart.GraphPane, CreateGraphics());
            zgChart.Refresh();
        }

        private void ChartControl_Resize(object sender, System.EventArgs e)
        {
            if (_chartModel == null || !_chartModel.HasEqualDimensions) return;

            Width = Height;
        }

        public void CrosshairSelectPoint(double X = double.NaN, double Y = double.NaN)
        {
            var pane = zgChart.GraphPane;
            pane.GraphObjList.Clear();

            if (!double.IsNaN(X) && !double.IsInfinity(X))
            {
                LineObj lineH = new LineObj(pane.XAxis.Scale.Min, Y, pane.XAxis.Scale.Max, Y);
                lineH.Line.Width = 1;
                lineH.Line.Color = Color.FromArgb(150, Color.Red);
                pane.GraphObjList.Add(lineH);
            }

            if (!double.IsNaN(Y) && !double.IsInfinity(Y))
            {
                LineObj lineV = new LineObj(X, pane.YAxis.Scale.Min, X, pane.YAxis.Scale.Max);
                lineV.Line.Width = 1;
                lineV.Line.Color = Color.FromArgb(150, Color.Red);
                pane.GraphObjList.Add(lineV);
            }

            zgChart.Refresh();
        }

        private void zgChart_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                zgChart.GraphPane.FindNearestPoint(new PointF(e.X, e.Y), out CurveItem nearestCurve, out int iNearest);
                ChartPointSelected?.Invoke(this, new ChartPointSelectedEventArgs(nearestCurve.Tag as Curve, iNearest));
            }
        }
    }
}
