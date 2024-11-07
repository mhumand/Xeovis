using DatabaseReader.Definitions.UI;
using DatabaseReader.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using ZedGraph;

namespace DatabaseReader.CustomControls
{
    public partial class ChartControl : UserControl
    {
        private ChartModel _chartModel;
        private LineObj _lineH;
        private LineObj _lineV;
        private TextObj _annotation;

        public ZedGraphControl Chart => zgChart;
        public EventHandler<ChartPointSelectedEventArgs> ChartPointSelected;
        public ChartControl()
        {
            InitializeComponent();
        }

        public void ClearPlot()
        {
            zgChart.GraphPane.CurveList.Clear();
            zgChart.GraphPane.XAxis.ResetAutoScale(zgChart.GraphPane, CreateGraphics());
            zgChart.GraphPane.YAxis.ResetAutoScale(zgChart.GraphPane, CreateGraphics());
            zgChart.Refresh();
        }

        public void Plot(ChartModel chartmodel)
        {
            _chartModel = chartmodel;

            zgChart.GraphPane.CurveList.Clear();

            foreach (var curveModel in _chartModel.Curves)
            {
                if (curveModel.IsHidden || curveModel.DataCount == 0)
                    continue;

                var curve = zgChart.GraphPane.AddCurve(curveModel.Name, curveModel.Xvals.ToArray(), curveModel.Yvals.ToArray(), curveModel.LineColor);
                curve.Tag = curveModel;
                curve.Line.IsAntiAlias = true;
                curve.Line.IsVisible = curveModel.IsLineVisible;
                curve.Line.Color = curveModel.LineColor;
                curve.Line.Width = curveModel.LineWidth;
                curve.Line.Style = curveModel.LineType;

                curve.Symbol.IsVisible = curveModel.IsMarkerVisible;
                curve.Symbol.Size = curveModel.MarkerSize;
                curve.Symbol.Fill.Color = curveModel.MarkerColor;
                curve.Symbol.Fill.IsVisible = curveModel.IsMarkerVisible;
                curve.Symbol.Fill.Type = FillType.Solid;
                curve.Symbol.Border.Color = curveModel.MarkerColor;
                curve.Symbol.Type = curveModel.MarkerType;

                curve.Label.FontSpec = zgChart.GraphPane.Legend.FontSpec.Clone();
            }

            // style the plot
            zgChart.GraphPane.Title.Text = _chartModel.Title;
            zgChart.GraphPane.Title.IsVisible = _chartModel.IsTitleVisible;
            zgChart.GraphPane.XAxis.Title.Text = _chartModel.XAxisLabel;
            zgChart.GraphPane.YAxis.Title.Text = _chartModel.YAxisLabel;

            zgChart.GraphPane.Legend.IsVisible = _chartModel.IsLegendVisible;
            zgChart.GraphPane.Legend.Border.IsVisible = false;

            zgChart.IsShowPointValues = true;

            // auto-axis and update the display
            zgChart.GraphPane.XAxis.ResetAutoScale(zgChart.GraphPane, CreateGraphics());
            zgChart.GraphPane.YAxis.ResetAutoScale(zgChart.GraphPane, CreateGraphics());
            zgChart.Refresh();
        }

        private void ChartControl_Resize(object sender, System.EventArgs e)
        {
            if (_chartModel == null || !_chartModel.HasEqualDimensions) return;

            Height = Width;
        }

        public void CrosshairSelectPoint(double X = double.NaN, double Y = double.NaN)
        {
            var pane = zgChart.GraphPane;
            pane.GraphObjList.Clear();

            if (!double.IsNaN(X) && !double.IsInfinity(X))
            {
                _lineH = new LineObj(pane.XAxis.Scale.Min, Y, pane.XAxis.Scale.Max, Y);
                _lineH.IsClippedToChartRect = true;
                _lineH.Line.Width = 1;
                _lineH.Line.Color = Color.FromArgb(150, Color.Red);
                pane.GraphObjList.Add(_lineH);
            }

            if (!double.IsNaN(Y) && !double.IsInfinity(Y))
            {
                _lineV = new LineObj(X, pane.YAxis.Scale.Min, X, pane.YAxis.Scale.Max);
                _lineV.IsClippedToChartRect = true;
                _lineV.Line.Width = 1;
                _lineV.Line.Color = Color.FromArgb(150, Color.Red);
                pane.GraphObjList.Add(_lineV);
            }

            zgChart.Refresh();
        }

        private void zgChart_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // curve visibility
                zgChart.GraphPane.FindNearestObject(new PointF(e.X, e.Y), CreateGraphics(), out object nearestObj, out int objIndex);
                if (nearestObj is Legend)
                {
                    SwitchCurveVisibility(objIndex);
                }

                // point selection
                zgChart.GraphPane.FindNearestPoint(new PointF(e.X, e.Y), out CurveItem nearestCurve, out int pointIndex);
                if (nearestCurve != null && nearestCurve.Tag != null && pointIndex >= 0)
                {
                    var curve = nearestCurve.Tag as CurveModel;
                    double x = curve.Xvals[pointIndex];
                    double y = curve.Yvals[pointIndex];
                    var nearestPtLocation = zgChart.GraphPane.GeneralTransform(x, y, CoordType.AxisXYScale);

                    if (Math.Abs(nearestPtLocation.X - e.X) <= 10 && Math.Abs(nearestPtLocation.Y - e.Y) <= 10)
                        ChartPointSelected?.Invoke(this, new ChartPointSelectedEventArgs(curve, pointIndex));
                    else
                        ChartPointSelected?.Invoke(this, new ChartPointSelectedEventArgs(null, -1));
                }
            }

            zgChart.Refresh();
        }

        private void SwitchCurveVisibility(int curveIndex)
        {
            bool alreadyVisible = zgChart.GraphPane.CurveList[curveIndex].IsVisible;
            zgChart.GraphPane.CurveList[curveIndex].IsVisible = !alreadyVisible;
            zgChart.GraphPane.CurveList[curveIndex].Label.FontSpec.FontColor = alreadyVisible ? Color.Gray : Color.Black;
        }

        private void zgChart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_lineH != null && _lineV != null)
                {
                    zgChart.GraphPane.GraphObjList.Remove(_lineH);
                    zgChart.GraphPane.GraphObjList.Remove(_lineV);
                    ChartPointSelected?.Invoke(this, new ChartPointSelectedEventArgs(null, -1));
                }
            }

            zgChart.Refresh();
        }

        private void zgChart_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            if (_lineH != null && _lineV != null)
            {
                var pane = zgChart.GraphPane;
                _lineH.Location.X = pane.XAxis.Scale.Min;
                _lineH.Location.Width = pane.XAxis.Scale.Max - pane.XAxis.Scale.Min;
                _lineV.Location.Y = pane.YAxis.Scale.Min;
                _lineV.Location.Height = pane.YAxis.Scale.Max - pane.YAxis.Scale.Min;
            }
        }
    }
}
