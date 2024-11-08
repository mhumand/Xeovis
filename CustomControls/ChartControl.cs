using DatabaseReader.Definitions.UI;
using DatabaseReader.Events;
using DatabaseReader.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;

namespace DatabaseReader.CustomControls
{
    public partial class ChartControl : UserControl
    {
        private ChartModel _chartModel;
        private GraphPane _graphPane;
        private LineObj _lineH;
        private LineObj _lineV;
        private TextObj _annotation;
        private string _visibilityFilterType;
        private List<string> _visibilityFilterKeys;
        private ContextMenuStrip _legendContextMenu;

        public ZedGraphControl Chart => zgChart;
        public EventHandler<ChartPointSelectedEventArgs> ChartPointSelected;
        public List<LineObj> LineObjects;
        public bool UseVisibilityFilterWithCurveLabel {  get; set; }

        public ChartControl()
        {
            InitializeComponent();
            _graphPane = zgChart.GraphPane;
            LineObjects = new List<LineObj>();
        }

        public void ClearPlot()
        {
            _graphPane.CurveList.Clear();
            _graphPane.XAxis.ResetAutoScale(_graphPane, CreateGraphics());
            _graphPane.YAxis.ResetAutoScale(_graphPane, CreateGraphics());
            zgChart.Refresh();
        }

        public void Plot(ChartModel chartmodel)
        {
            _chartModel = chartmodel;
            _graphPane.CurveList.Clear();

            foreach (var curveModel in _chartModel.Curves)
            {
                if (curveModel.IsHidden || curveModel.DataCount == 0)
                    continue;

                var curve = _graphPane.AddCurve(curveModel.Name, curveModel.Xvals.ToArray(), curveModel.Yvals.ToArray(), curveModel.LineColor);
                curve.Tag = curveModel;
                int transparency = (int)(255 * (1 - curveModel.Transparency / 100.0));

                curve.Line.IsAntiAlias = true;
                curve.Line.IsVisible = curveModel.IsLineVisible;
                curve.Line.Color = Color.FromArgb(transparency, curveModel.LineColor);
                curve.Line.Width = curveModel.LineWidth;
                curve.Line.Style = curveModel.LineType;

                curve.Symbol.IsVisible = curveModel.IsMarkerVisible;
                curve.Symbol.Size = curveModel.MarkerSize;
                curve.Symbol.Fill.Color = Color.FromArgb(transparency, curveModel.MarkerColor);
                curve.Symbol.Fill.IsVisible = curveModel.IsMarkerVisible;
                curve.Symbol.Fill.Type = FillType.Solid;
                curve.Symbol.Border.Color = Color.FromArgb(transparency, curveModel.MarkerColor);
                curve.Symbol.Type = curveModel.MarkerType;

                curve.Label.FontSpec = _graphPane.Legend.FontSpec.Clone();
            }


            // style the plot
            _graphPane.Title.Text = _chartModel.Title;
            _graphPane.Title.IsVisible = _chartModel.IsTitleVisible;
            _graphPane.XAxis.Title.Text = _chartModel.XAxisLabel;
            _graphPane.YAxis.Title.Text = _chartModel.YAxisLabel;

            _graphPane.Legend.IsVisible = _chartModel.IsLegendVisible;
            _graphPane.Legend.Border.IsVisible = false;

            zgChart.IsShowPointValues = true;

            // auto-axis and update the display
            _graphPane.XAxis.ResetAutoScale(_graphPane, CreateGraphics());
            _graphPane.YAxis.ResetAutoScale(_graphPane, CreateGraphics());
            zgChart.Refresh();
        }

        private void ChartControl_Resize(object sender, System.EventArgs e)
        {
            if (_chartModel == null || !_chartModel.HasEqualDimensions) return;

            Height = Width;
        }

        public void CrosshairSelectPoint(double X = double.NaN, double Y = double.NaN)
        {
            _graphPane.GraphObjList.Remove(_lineV);
            _graphPane.GraphObjList.Remove(_lineH);

            if (!double.IsNaN(X) && !double.IsInfinity(X))
            {
                _lineH = new LineObj(_graphPane.XAxis.Scale.Min, Y, _graphPane.XAxis.Scale.Max, Y);
                _lineH.IsClippedToChartRect = true;
                _lineH.Line.Width = 1;
                _lineH.Line.Color = Color.FromArgb(150, Color.Red);
                _graphPane.GraphObjList.Add(_lineH);
            }

            if (!double.IsNaN(Y) && !double.IsInfinity(Y))
            {
                _lineV = new LineObj(X, _graphPane.YAxis.Scale.Min, X, _graphPane.YAxis.Scale.Max);
                _lineV.IsClippedToChartRect = true;
                _lineV.Line.Width = 1;
                _lineV.Line.Color = Color.FromArgb(150, Color.Red);
                _graphPane.GraphObjList.Add(_lineV);
            }

            zgChart.Refresh();
        }

        private void zgChart_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // curve visibility
                _graphPane.FindNearestObject(new PointF(e.X, e.Y), CreateGraphics(), out object nearestObj, out int objIndex);
                if (nearestObj is Legend && objIndex.IsBetween(0, _graphPane.CurveList.Count - 1))
                {
                    SwitchCurveVisibility(objIndex);
                }

                // point selection
                _graphPane.FindNearestPoint(new PointF(e.X, e.Y), out CurveItem nearestCurve, out int pointIndex);
                if (nearestCurve != null && nearestCurve.Tag != null && pointIndex >= 0)
                {
                    var curve = nearestCurve.Tag as CurveModel;
                    double x = curve.Xvals[pointIndex];
                    double y = curve.Yvals[pointIndex];
                    var nearestPtLocation = _graphPane.GeneralTransform(x, y, CoordType.AxisXYScale);

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
            var curve = _graphPane.CurveList[curveIndex] as LineItem;
            bool alreadyVisible = curve.IsVisible;
            curve.IsVisible = !alreadyVisible;

            int originalTransparency = (int)(255 * (1 - (curve.Tag as CurveModel).Transparency / 100.0));
            int tempTransparency = (int)(255 * (1 - 80 / 100.0));

            var labelColor = curve.Label.FontSpec.FontColor;
            var newLabelColor = alreadyVisible ? Color.FromArgb(tempTransparency, labelColor) : Color.FromArgb(255, labelColor);

            var pointColor = curve.Color;
            var newPointColor = alreadyVisible ? Color.FromArgb(tempTransparency, pointColor) : Color.FromArgb(originalTransparency, pointColor);

            curve.Label.FontSpec.FontColor = newLabelColor;
            curve.Color = newPointColor;
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
                _lineH.Location.X = _graphPane.XAxis.Scale.Min;
                _lineH.Location.Width = _graphPane.XAxis.Scale.Max - _graphPane.XAxis.Scale.Min;
                _lineV.Location.Y = _graphPane.YAxis.Scale.Min;
                _lineV.Location.Height = _graphPane.YAxis.Scale.Max - _graphPane.YAxis.Scale.Min;
            }
        }

        private void zgChart_ContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            menuStrip.BackColor = Color.FromArgb(240, 240, 240);
            _graphPane.FindNearestObject(mousePt, CreateGraphics(), out object nearestObj, out int objIndex);

            if (nearestObj is Legend)
                MakeLegendContextMenu(menuStrip);
            else
                MakeChartContexMenu(menuStrip);
        }

        private void MakeChartContexMenu(ContextMenuStrip menuStrip)
        {
            menuStrip.ShowImageMargin = true;

            var toBeRemovedNames = new List<string>() { "print", "page_setup", "unzoom", "undo_all" };
            var toBeRemovedItems = new List<ToolStripItem>();
            foreach (ToolStripItem item in menuStrip.Items)
                if (toBeRemovedNames.Contains(item.Name))
                    toBeRemovedItems.Add(item);

            foreach (var item in toBeRemovedItems)
                menuStrip.Items.Remove(item);

            var showLegendMenuItem = new ToolStripMenuItem("Show Legend", null, ShowLegendMenuItem_Clicked);
            showLegendMenuItem.Checked = _graphPane.Legend.IsVisible;

            menuStrip.Items.Add(showLegendMenuItem);
        }

        private void ShowLegendMenuItem_Clicked(object sender, EventArgs e)
        {
            _graphPane.Legend.IsVisible = !_graphPane.Legend.IsVisible;
            zgChart.Refresh();
        }

        private void MakeLegendContextMenu(ContextMenuStrip menuStrip)
        {
            menuStrip.Items.Clear();
            menuStrip.ShowImageMargin = false;

            var showAllItem = new ToolStripMenuItem("Show All", null, ShowAllItem_Click);
            menuStrip.Items.Add(showAllItem);

            var hideAllItem = new ToolStripMenuItem("Hide All", null, HideAllItem_Click);
            menuStrip.Items.Add(hideAllItem);

            var invertVisibility = new ToolStripMenuItem("Invert Visibility", null, InvertVisibility_Click);
            menuStrip.Items.Add(invertVisibility);

            menuStrip.Items.Add(new ToolStripSeparator());

            if (UseVisibilityFilterWithCurveLabel)
            {
                var filterItem = new ToolStripMenuItem("Filter Visitbility");
                var filterMenu = (filterItem.DropDown as ToolStripDropDownMenu);
                filterMenu.ShowImageMargin = false;
                filterMenu.BackColor = menuStrip.BackColor;
                filterMenu.AutoSize = false;
                filterMenu.Width = 170;
                filterMenu.Height = 60;

                var filterTypeCmbx = new ToolStripComboBox();
                filterTypeCmbx.DropDownStyle = ComboBoxStyle.DropDownList;
                filterTypeCmbx.FlatStyle = FlatStyle.System;
                filterTypeCmbx.AutoSize = false;
                filterTypeCmbx.Width = filterMenu.Width - 20;
                filterTypeCmbx.Items.Add("Show those containing");
                filterTypeCmbx.Items.Add("Hide those containing");
                filterTypeCmbx.SelectedIndexChanged += FilterTypeCmbx_SelectedIndexChanged;

                if (_visibilityFilterType == null)
                    filterTypeCmbx.SelectedIndex = 0;
                else
                    filterTypeCmbx.SelectedItem = _visibilityFilterType;

                filterItem.DropDownItems.Add(filterTypeCmbx);

                var filterTxtBox = new ToolStripTextBox();
                if (_visibilityFilterKeys != null && _visibilityFilterKeys.Count > 0)
                    filterTxtBox.Text = string.Join(" ", _visibilityFilterKeys.ToArray());

                filterTxtBox.AutoSize = false;
                filterTxtBox.Width = filterMenu.Width - 20;
                filterTxtBox.KeyDown += filterTxtBox_KeyDown;
                filterItem.DropDownItems.Add(filterTxtBox);
                menuStrip.Items.Add(filterItem);
            }

            _legendContextMenu = menuStrip;
        }

        private void InvertVisibility_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _graphPane.CurveList.Count; i++)
            {
                var curve = _graphPane.CurveList[i];
                if ((curve.Tag as CurveModel).CurveType == Enums.UI.CurveType.FixedLine) continue;
                SwitchCurveVisibility(i);
            }

            zgChart.Refresh();
            _visibilityFilterKeys = null;
        }

        private void FilterTypeCmbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            _visibilityFilterType = (sender as ToolStripComboBox).SelectedItem.ToString();
        }

        private void HideAllItem_Click(object sender, EventArgs e)
        {
            HideAllCurves();
            _visibilityFilterKeys = null;
        }

        private void HideAllCurves()
        {
            for (int i = 0; i < _graphPane.CurveList.Count; i++)
            {
                var curve = _graphPane.CurveList[i];
                if ((curve.Tag as CurveModel).CurveType == Enums.UI.CurveType.FixedLine) continue;

                curve.IsVisible = true;
                SwitchCurveVisibility(i);
            }

            zgChart.Refresh();
        }

        private void ShowAllItem_Click(object sender, EventArgs e)
        {
            ShowAllCurves();
            _visibilityFilterKeys = null;
        }

        private void ShowAllCurves()
        {
            for (int i = 0; i < _graphPane.CurveList.Count; i++)
            {
                var curve = _graphPane.CurveList[i];
                if ((curve.Tag as CurveModel).CurveType == Enums.UI.CurveType.FixedLine) continue;

                curve.IsVisible = false;
                SwitchCurveVisibility(i);
            }

            zgChart.Refresh();
        }

        private void filterTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                var textBox = sender as ToolStripTextBox;
                if (textBox == null) return;
                var filterStr = textBox.Text;
                var separators = new char[] { ' ', ',' };
                var keys = filterStr.Split(separators).ToList();
                keys.RemoveAll(l => string.IsNullOrWhiteSpace(l));
                if (keys.Count == 0) return;

                if (_visibilityFilterType.ToLower().Contains("show"))
                    HideAllCurves();
                else
                    ShowAllCurves();

                for (int i = 0; i < _graphPane.CurveList.Count; i++)
                {
                    if (keys.Any(k => _graphPane.CurveList[i].Label.Text.Contains(k)))
                        SwitchCurveVisibility(i);
                }

                _visibilityFilterKeys = keys;
                _legendContextMenu.Close();

                zgChart.Refresh();
            }
        }
    }
}
