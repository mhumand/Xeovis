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
        private string _labelFilterType;
        private List<string> _visibilityFilterKeys;
        private ContextMenuStrip _legendContextMenu;

        public ZedGraphControl Chart => zgChart;
        public EventHandler<ChartPointSelectedEventArgs> ChartPointSelected;
        public List<LineObj> LineObjects;
        public bool UseLabelFiltering { get; set; }
        public Dictionary<string, List<string>> SpecialFilters;

        public ChartControl()
        {
            InitializeComponent();
            _graphPane = zgChart.GraphPane;
            LineObjects = new List<LineObj>();
            SpecialFilters = new Dictionary<string, List<string>>();
        }

        public void ClearPlot()
        {
            _graphPane.CurveList.Clear();
            _graphPane.XAxis.ResetAutoScale(_graphPane, CreateGraphics());
            _graphPane.YAxis.ResetAutoScale(_graphPane, CreateGraphics());
            _graphPane.Title.Text = null;
            _graphPane.XAxis.Title.Text = null;
            _graphPane.YAxis.Title.Text = null;
            zgChart.Refresh();
        }

        public void Plot(ChartModel chartmodel)
        {
            _chartModel = chartmodel;
            _graphPane.CurveList.Clear();
            _visibilityFilterKeys = null;

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

            SetPlotStyle();

            zgChart.IsShowPointValues = true;

            // auto-axis and update the display
            _graphPane.XAxis.ResetAutoScale(_graphPane, CreateGraphics());
            _graphPane.YAxis.ResetAutoScale(_graphPane, CreateGraphics());
            zgChart.Refresh();
        }

        private void SetPlotStyle()
        {
            string fontFamily = "Segoe UI";

            _graphPane.Border.Width = 3;
            _graphPane.Border.IsAntiAlias = true;
            _graphPane.Chart.Border.Width = 2;

            _graphPane.Title.Text = _chartModel.Title;
            _graphPane.Title.IsVisible = _chartModel.IsTitleVisible;

            _graphPane.XAxis.Title.Text = _chartModel.XAxisLabel;
            _graphPane.YAxis.Title.Text = _chartModel.YAxisLabel;

            // legend
            _graphPane.Legend.IsVisible = _chartModel.IsLegendVisible;
            _graphPane.Legend.Position = _chartModel.LegendPosition;
            _graphPane.Legend.IsHStack = true;
            _graphPane.Legend.Border.IsVisible = false;
            _graphPane.Legend.Fill.Color = Color.Transparent;

            // tics
            _graphPane.XAxis.MajorTic.IsOpposite = false;
            _graphPane.XAxis.MinorTic.IsOpposite = false;
            _graphPane.YAxis.MajorTic.IsOpposite = false;
            _graphPane.YAxis.MinorTic.IsOpposite = false;
            _graphPane.XAxis.MinorTic.IsOutside = false;
            _graphPane.YAxis.MinorTic.IsOutside = false;
            _graphPane.XAxis.MajorTic.PenWidth = 2;
            _graphPane.XAxis.MinorTic.PenWidth = 2;
            _graphPane.YAxis.MajorTic.PenWidth = 2;
            _graphPane.YAxis.MinorTic.PenWidth = 2;

            // scale
            _graphPane.XAxis.Scale.FontSpec.IsBold = true;
            _graphPane.XAxis.Scale.FontSpec.Family = fontFamily;
            _graphPane.YAxis.Scale.FontSpec.IsBold = true;
            _graphPane.YAxis.Scale.FontSpec.Family = fontFamily;
            _graphPane.XAxis.Title.FontSpec.Family = fontFamily;
            _graphPane.YAxis.Title.FontSpec.Family = fontFamily;

            foreach (var curve in _graphPane.CurveList)
            {
                curve.Label.FontSpec.IsBold = true;
                curve.Label.FontSpec.Family = fontFamily;
            }

        }

        private void ChartControl_Resize(object sender, System.EventArgs e)
        {
            if (_chartModel == null || !_chartModel.HasEqualDimensions) return;

            int height = Width;
            //if (_graphPane.Legend.IsVisible && (int)_graphPane.Legend.Rect.Height > 2 * _graphPane.Margin.Top)
            //    height += (int)_graphPane.Legend.Rect.Height;

            Height = height;
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
            _graphPane.FindNearestObject(new PointF(e.X, e.Y), CreateGraphics(), out object nearestObj, out int objIndex);
            _graphPane.FindNearestPoint(new PointF(e.X, e.Y), out CurveItem nearestCurve, out int pointIndex);

            if (e.Button == MouseButtons.Left)
            {
                if (nearestObj is Legend && objIndex.IsBetween(0, _graphPane.CurveList.Count - 1))
                    SwitchCurveVisibility(objIndex);

                // point selection
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
            else if (e.Button == MouseButtons.Middle)
            {
                if (nearestObj is Legend && objIndex.IsBetween(0, _graphPane.CurveList.Count - 1))
                    ShowSelectedCurveOnly(objIndex);
            }

            zgChart.Refresh();
        }

        private void SwitchCurveVisibility(int curveIndex)
        {
            if (curveIndex < 0 || curveIndex >= _graphPane.CurveList.Count) return;
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
                MakeLegendContextMenu(menuStrip, objIndex);
            else
                MakeChartContexMenu(menuStrip);
        }

        private void MakeChartContexMenu(ContextMenuStrip menuStrip)
        {
            menuStrip.ShowImageMargin = true;

            var toBeRemovedNames = new List<string>() { "print", "page_setup", "unzoom", "undo_all", "set_default" };
            var toBeRemovedItems = new List<ToolStripItem>();
            foreach (ToolStripItem item in menuStrip.Items)
                if (toBeRemovedNames.Contains(item.Name))
                    toBeRemovedItems.Add(item);

            foreach (var item in toBeRemovedItems)
                menuStrip.Items.Remove(item);

            var showLegendMenuItem = new ToolStripMenuItem("Show Legend");
            showLegendMenuItem.Checked = _graphPane.Legend.IsVisible;
            showLegendMenuItem.Click += (e, arg) =>
            {
                _graphPane.Legend.IsVisible = !_graphPane.Legend.IsVisible;
                zgChart.Refresh();
            };
            menuStrip.Items.Add(showLegendMenuItem);

            menuStrip.Items.Add(new ToolStripSeparator());
            var resetZoomMenuItem = new ToolStripMenuItem("Reset Zoom");
            resetZoomMenuItem.Click += (e, arg) => zgChart.ZoomOutAll(_graphPane);
            menuStrip.Items.Add(resetZoomMenuItem);
        }

        private void MakeLegendContextMenu(ContextMenuStrip menuStrip, int objIndex)
        {
            menuStrip.Items.Clear();
            menuStrip.ShowImageMargin = false;

            AddGeneralVisibilityMenuItems(menuStrip, objIndex);
            menuStrip.Items.Add(new ToolStripSeparator());
            AddLegendSettingsMenuItems(menuStrip);
            menuStrip.Items.Add(new ToolStripSeparator());

            if (UseLabelFiltering)
                AddFilterLabelsMenuItem(menuStrip);

            if (SpecialFilters != null && SpecialFilters.Count > 0)
                AddSpecialFiltersMenuItem(menuStrip);

            _legendContextMenu = menuStrip;
        }

        private void AddGeneralVisibilityMenuItems(ContextMenuStrip menuStrip, int objIndex)
        {
            var showAllMenuItem = new ToolStripMenuItem("Show All", null, ShowAllMenuItem_Click);
            showAllMenuItem.Enabled = _graphPane.CurveList.Any(c => !c.IsVisible);
            menuStrip.Items.Add(showAllMenuItem);

            var hideAllMenuItem = new ToolStripMenuItem("Hide All", null, HideAllMenuItem_Click);
            hideAllMenuItem.Enabled = _graphPane.CurveList.Any(c => c.IsVisible && (c.Tag as CurveModel).CurveType == Enums.UI.CurveType.Normal);
            menuStrip.Items.Add(hideAllMenuItem);

            var invertVisibilityMenuItem = new ToolStripMenuItem("Invert Visibility", null, InvertVisibilityMenuItem_Click);
            invertVisibilityMenuItem.Enabled = hideAllMenuItem.Enabled && showAllMenuItem.Enabled;
            menuStrip.Items.Add(invertVisibilityMenuItem);

            if (objIndex >= 0 && objIndex < _graphPane.CurveList.Count)
            {
                var showThisOnlyMenuItem = new ToolStripMenuItem("Show This Only", null, ShowThisOnlyMenuItem_Click);
                showThisOnlyMenuItem.Tag = objIndex;
                menuStrip.Items.Add(showThisOnlyMenuItem);
            }

            if (_graphPane.CurveList.Any(c => !c.IsVisible))
            {
                var removeHiddenCurvesMenuItem = new ToolStripMenuItem("Remove Hidden Curves");
                removeHiddenCurvesMenuItem.Click += (e, arg) =>
                {
                    _graphPane.CurveList.RemoveAll(c => !c.IsVisible);
                    zgChart.Refresh();
                };

                menuStrip.Items.Add(removeHiddenCurvesMenuItem);
            }
        }

        private void AddLegendSettingsMenuItems(ContextMenuStrip menuStrip)
        {
            var legendPositionMenuItem = new ToolStripMenuItem("Legend Position", null, ShowAllMenuItem_Click);
            var legendPositionMenu = legendPositionMenuItem.DropDown as ToolStripDropDownMenu;
            legendPositionMenu.BackColor = menuStrip.BackColor;

            var positions = Enum.GetValues(typeof(LegendPos));
            foreach (LegendPos position in positions)
            {
                var positionMenuItem = new ToolStripMenuItem(position.ToString());
                positionMenuItem.Tag = position;
                positionMenuItem.Checked = _graphPane.Legend.Position == position;
                positionMenuItem.Click += PositionMenuItem_Click;
                legendPositionMenuItem.DropDownItems.Add(positionMenuItem);
            }

            menuStrip.Items.Add(legendPositionMenuItem);
        }

        private void PositionMenuItem_Click(object sender, EventArgs e)
        {
            var position = (LegendPos)(sender as ToolStripMenuItem).Tag;
            _chartModel.LegendPosition = position;
            _graphPane.Legend.Position = position;
            zgChart.Refresh();
        }

        private void ShowThisOnlyMenuItem_Click(object sender, EventArgs e)
        {
            var index = (int)(sender as ToolStripMenuItem).Tag;
            ShowSelectedCurveOnly(index);
        }

        private void ShowSelectedCurveOnly(int curveIndex)
        {
            HideAllCurves(refresh: false);
            SwitchCurveVisibility(curveIndex);
            zgChart.Refresh();
        }

        private void AddFilterLabelsMenuItem(ContextMenuStrip menuStrip)
        {
            var filterLabelsItem = new ToolStripMenuItem("Filter Labels");
            var filterLabelsMenu = filterLabelsItem.DropDown as ToolStripDropDownMenu;
            filterLabelsMenu.ShowImageMargin = false;
            filterLabelsMenu.BackColor = menuStrip.BackColor;
            filterLabelsMenu.AutoSize = false;
            filterLabelsMenu.Width = 170;
            filterLabelsMenu.Height = 60;

            var filterLabelTypeCmbx = new ToolStripComboBox();
            filterLabelTypeCmbx.DropDownStyle = ComboBoxStyle.DropDownList;
            filterLabelTypeCmbx.FlatStyle = FlatStyle.System;
            filterLabelTypeCmbx.AutoSize = false;
            filterLabelTypeCmbx.Width = filterLabelsMenu.Width - 20;
            filterLabelTypeCmbx.Items.Add("Show those containing");
            filterLabelTypeCmbx.Items.Add("Hide those containing");
            filterLabelTypeCmbx.SelectedIndexChanged += FilterLabelTypeCmbx_SelectedIndexChanged;

            if (_labelFilterType == null)
                filterLabelTypeCmbx.SelectedIndex = 0;
            else
                filterLabelTypeCmbx.SelectedItem = _labelFilterType;

            filterLabelsItem.DropDownItems.Add(filterLabelTypeCmbx);

            var filterTxtBox = new ToolStripTextBox();
            if (_visibilityFilterKeys != null && _visibilityFilterKeys.Count > 0)
                filterTxtBox.Text = string.Join(" ", _visibilityFilterKeys.ToArray());

            filterTxtBox.AutoSize = false;
            filterTxtBox.Width = filterLabelsMenu.Width - 20;
            filterTxtBox.KeyDown += filterLabelTxtBox_KeyDown;
            filterLabelsItem.DropDownItems.Add(filterTxtBox);
            menuStrip.Items.Add(filterLabelsItem);
        }

        private void AddSpecialFiltersMenuItem(ContextMenuStrip menuStrip)
        {
            var specialFiltersMenuItem = new ToolStripMenuItem("Special Filters");
            var filterLabelsMenu = specialFiltersMenuItem.DropDown as ToolStripDropDownMenu;
            filterLabelsMenu.ShowImageMargin = false;
            filterLabelsMenu.BackColor = menuStrip.BackColor;
            
            foreach (var filter in SpecialFilters)
            {
                var specialFilterMenuItem = new ToolStripMenuItem(filter.Key);
                specialFilterMenuItem.Click += SpecialFilterMenuItem_Click;
                specialFilterMenuItem.Tag = filter.Value;
                specialFiltersMenuItem.DropDownItems.Add(specialFilterMenuItem);
            }

            menuStrip.Items.Add(specialFiltersMenuItem);
        }

        private void SpecialFilterMenuItem_Click(object sender, EventArgs e)
        {
            var keys = (sender as ToolStripMenuItem).Tag as List<string>;
            if (keys == null || keys.Count == 0) return;

            //todo: currently is for show-only. Must be generalized.
            HideAllCurves(refresh: false);
            foreach (var curve in _graphPane.CurveList.Where(c => (c.Tag as CurveModel).CurveType == Enums.UI.CurveType.Normal))
            {
                string label = curve.Label.Text;
                var labelParts = label.Split(new char[] { ' ', ',' , '+' }).ToList();
                labelParts.RemoveAll(p => string.IsNullOrWhiteSpace(p));
                bool mustBeShown = true;
                foreach (var part in labelParts)
                {
                    if (!keys.Contains(part))
                    {
                        mustBeShown = false;
                        break;
                    }
                }

                if (mustBeShown)
                    SwitchCurveVisibility(_graphPane.CurveList.IndexOf(curve));
            }

            zgChart.Refresh();
            _visibilityFilterKeys = null;
        }

        private void InvertVisibilityMenuItem_Click(object sender, EventArgs e)
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

        private void FilterLabelTypeCmbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            _labelFilterType = (sender as ToolStripComboBox).SelectedItem.ToString();
        }

        private void HideAllMenuItem_Click(object sender, EventArgs e)
        {
            HideAllCurves();
            _visibilityFilterKeys = null;
        }

        private void HideAllCurves(bool refresh = true)
        {
            for (int i = 0; i < _graphPane.CurveList.Count; i++)
            {
                var curve = _graphPane.CurveList[i];
                if ((curve.Tag as CurveModel).CurveType == Enums.UI.CurveType.FixedLine) continue;

                curve.IsVisible = true;
                SwitchCurveVisibility(i);
            }

            if (refresh)
                zgChart.Refresh();
        }

        private void ShowAllMenuItem_Click(object sender, EventArgs e)
        {
            ShowAllCurves();
            _visibilityFilterKeys = null;
        }

        private void ShowAllCurves(bool refresh = true)
        {
            for (int i = 0; i < _graphPane.CurveList.Count; i++)
            {
                var curve = _graphPane.CurveList[i];
                if ((curve.Tag as CurveModel).CurveType == Enums.UI.CurveType.FixedLine) continue;

                curve.IsVisible = false;
                SwitchCurveVisibility(i);
            }

            if (refresh)
                zgChart.Refresh();
        }

        private void filterLabelTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                var textBox = sender as ToolStripTextBox;
                if (textBox == null) return;
                var filterStr = textBox.Text;
                var separators = new char[] { ' ', ',' };
                var keys = filterStr.Split(separators).ToList();
                keys.RemoveAll(l => string.IsNullOrWhiteSpace(l));
                if (keys.Count == 0)
                {
                    ShowAllCurves(refresh: false);
                    _visibilityFilterKeys = null;
                }
                else
                {
                    if (_labelFilterType.ToLower().Contains("show"))
                        HideAllCurves(refresh: false);
                    else
                        ShowAllCurves(refresh: false);

                    for (int i = 0; i < _graphPane.CurveList.Count; i++)
                    {
                        if (keys.Any(k => _graphPane.CurveList[i].Label.Text.Contains(k)))
                            SwitchCurveVisibility(i);
                    }

                    _visibilityFilterKeys = keys;
                }

                _legendContextMenu.Close();

                zgChart.Refresh();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
