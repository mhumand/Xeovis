using Avalonia.Controls;
using ScottPlot;
using System;
using System.Collections.Generic;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.ViewModels;
using ScottPlot.Plottables;
using Xeovis.UI.Models.Events;
using System.Linq;
using Avalonia.Media.Imaging;

namespace Xeovis.UI;

public partial class ChartControl : UserControl
{
    private ChartControlViewModel _viewModel = new ChartControlViewModel();

    public ChartControlViewModel ViewModel
    {
        get { return _viewModel; }
        set { _viewModel = value; }
    }

    private ChartModel _chartModel => _viewModel.ChartModel;
    private Dictionary<Guid, IPlottable> _plottedCurves = new Dictionary<Guid, IPlottable>();
    private Plot _plot => scottPlot.Plot;
    private Crosshair? _selectedPointCrosshair;

    public event EventHandler<ChartPointSelectedEventArgs>? ChartPointSelected;

    public ChartControl()
    {
        InitializeComponent();
        SizeChanged += ChartControl_SizeChanged;

        scottPlot.UserInputProcessor.LeftClickDragPan(false);
        scottPlot.UserInputProcessor.DoubleLeftClickBenchmark(false);
        scottPlot.UserInputProcessor.RightClickDragZoom(false);
        scottPlot.UserInputProcessor.RemoveAll<ScottPlot.Interactivity.UserActionResponses.MouseDragZoomRectangle>();

        btnRectangleZoom.IsCheckedChanged += btnRectangleZoom_IsCheckedChanged;
        btnResetZoom.Click += btnResetZoom_Click;
        btnZoomIn.Click += btnZoomIn_Click;
        btnZoomOut.Click += btnZoomOut_Click;
        btnLegend.IsCheckedChanged += btnLegend_IsCheckedChanged;
    }

    public bool PopulateChart(ChartModel chartModel, bool reBuild = true, bool resetZoom = true)
    {
        if (chartModel == null) return false;
        _viewModel = new ChartControlViewModel(chartModel);

        ClearPlot(reBuild);

        foreach (var curve in _chartModel.Curves)
        {
            if (!curve.IsVisible || curve.DataCount == 0) continue;

            bool successful = curve.CurveType switch
            {
                CurveType.Scatter => AddScatter(curve),
                CurveType.Line => AddLine(curve),
                CurveType.Crosshair => AddCrosshair(curve),
                _ => false,
            };

            if (!successful)
            {

            }
        }

        SetProperties();
        SetContextMenu();

        if (resetZoom)
            _plot.Axes.AutoScale();

        scottPlot.Refresh();
        return true;
    }

    private bool AddScatter(CurveModel curve)
    {
        var scatter = _plot.Add.Scatter(curve.Xvals.ToArray(), curve.Yvals.ToArray());
        scatter.LineColor = Color.FromColor(curve.LineColor);
        SetLineStyle(scatter.LineStyle, curve);
        scatter.MarkerColor = Color.FromColor(curve.MarkerColor);
        SetMarkerStyle(scatter.MarkerStyle, curve);
        scatter.LegendText = curve.IsVisibleInLegend ? curve.Name : null;

        _plottedCurves.Add(curve.Id, scatter);
        return true;
    }

    private bool AddLine(CurveModel curve)
    {
        if (curve.Xvals.Count < 1 || curve.Yvals.Count < 1) return false;

        var line = _plot.Add.Line(curve.Xvals[0], curve.Yvals[0], curve.Xvals[1], curve.Yvals[1]);
        line.LineColor = Color.FromColor(curve.LineColor);
        SetLineStyle(line.LineStyle, curve);
        line.MarkerColor = Color.FromColor(curve.MarkerColor);
        SetMarkerStyle(line.MarkerStyle, curve);
        line.LegendText = curve.IsVisibleInLegend ? curve.Name : null;

        _plottedCurves.Add(curve.Id, line);
        return true;
    }

    private bool AddCrosshair(CurveModel curve)
    {
        var crosshair = _plot.Add.Crosshair(curve.Xvals[0], curve.Yvals[0]);
        crosshair.LineColor = Color.FromColor(curve.LineColor);
        crosshair.LinePattern = GetScottPlotLinePattern(curve.LineType);
        crosshair.IsVisible = curve.IsLineVisible;
        crosshair.LineWidth = curve.LineWidth;
        crosshair.MarkerColor = Color.FromColor(curve.MarkerColor);
        SetMarkerStyle(crosshair.MarkerStyle, curve);

        _plottedCurves.Add(curve.Id, crosshair);
        return true;
    }

    private void SetContextMenu()
    {
        scottPlot.Menu?.Clear();

        //scottPlot.Menu?.AddSeparator();

        scottPlot.Menu?.Add("Reset Zoom", (plot) =>
        {
            plot.Axes.AutoScale();
        });
    }

    private void SetProperties()
    {
        //_plot.Axes.SquareUnits(_chartModel.HasEqualDimensions);
        _plot.Axes.Left.Label.Text = _chartModel.YAxis.Label;
        _plot.Axes.Left.Label.IsVisible = _chartModel.YAxis.IsLabelVisible;
        _plot.Axes.Left.Label.AntiAliasText = true;
        _plot.Axes.Bottom.Label.Text = _chartModel.XAxis.Label;
        _plot.Axes.Bottom.Label.IsVisible = _chartModel.XAxis.IsLabelVisible;
        _plot.Axes.Bottom.Label.AntiAliasText = true;
        SetAxisLimits(_chartModel.XAxis.MinLimit, _chartModel.YAxis.MinLimit, _chartModel.XAxis.MaxLimit, _chartModel.YAxis.MaxLimit);

        _plot.Axes.Title.Label.Text = _chartModel.Title;
        _plot.Axes.Title.Label.FontSize = 20;
        _plot.Axes.Title.IsVisible = _chartModel.IsTitleVisible;

        _plot.Legend.IsVisible = _chartModel.Legend.IsVisible;
        _plot.Legend.Alignment = GetScottPlotLegendAlignment(_chartModel.Legend.Position);
        _plot.Legend.BackgroundColor = Color.FromColor(System.Drawing.Color.Transparent);
        _plot.Legend.ShadowColor = Color.FromColor(System.Drawing.Color.Transparent);
        _plot.Legend.OutlineWidth = 0.5f;
        btnLegend.IsChecked = _plot.Legend.IsVisible;

        _plot.Grid.IsVisible = _chartModel.IsGridVisible;
        _plot.Font.Set(Fonts.Serif);
    }

    private void SetLineStyle(LineStyle lineStyle, CurveModel curve)
    {
        lineStyle.IsVisible = curve.IsLineVisible;
        lineStyle.Width = curve.LineWidth;
        lineStyle.Pattern = GetScottPlotLinePattern(curve.LineType);
        lineStyle.AntiAlias = curve.IsLineAntiAlis;
    }

    private void SetMarkerStyle(MarkerStyle markerStyle, CurveModel curve)
    {
        markerStyle.Size = curve.MarkerSize;
        markerStyle.IsVisible = curve.IsMarkerVisible;
        markerStyle.Shape = GetScottPlotMarkerShape(curve.MarkerType);
    }

    private LinePattern GetScottPlotLinePattern(LineType linePattern)
    {
        return linePattern switch
        {
            LineType.Solid => LinePattern.Solid,
            LineType.Dashed => LinePattern.Dashed,
            LineType.Dotted => LinePattern.Dotted,
            _ => LinePattern.Solid,
        };
    }

    private MarkerShape GetScottPlotMarkerShape(MarkerType markerType)
    {
        return markerType switch
        {
            MarkerType.OpenCircle => MarkerShape.OpenCircle,
            MarkerType.OpenSquare => MarkerShape.OpenSquare,
            MarkerType.OpenTriangleUp => MarkerShape.OpenTriangleUp,
            MarkerType.OpenTriangleDown => MarkerShape.OpenTriangleDown,
            MarkerType.OpenDiamond => MarkerShape.OpenDiamond,
            MarkerType.FilledCircle => MarkerShape.FilledCircle,
            MarkerType.FilledSquare => MarkerShape.FilledSquare,
            MarkerType.FilledTriangleUp => MarkerShape.FilledTriangleUp,
            MarkerType.FilledTriangleDown => MarkerShape.FilledTriangleDown,
            MarkerType.FilledDiamond => MarkerShape.FilledDiamond,
            MarkerType.Asterisk => MarkerShape.Asterisk,
            MarkerType.Cross => MarkerShape.Cross,
            MarkerType.HorizontalBar => MarkerShape.HorizontalBar,
            MarkerType.VerticalBar => MarkerShape.VerticalBar,
            MarkerType.HashTag => MarkerShape.HashTag,
            _ => MarkerShape.FilledCircle,
        };
    }

    private Alignment GetScottPlotLegendAlignment(LegendPosition legendPosition)
    {
        return legendPosition switch
        {
            LegendPosition.InsideTopLeft => Alignment.UpperLeft,
            LegendPosition.InsideTopRight => Alignment.UpperRight,
            LegendPosition.InsideTopCenter => Alignment.UpperCenter,
            LegendPosition.InsideBottomLeft => Alignment.LowerLeft,
            LegendPosition.InsideBottomRight => Alignment.LowerRight,
            LegendPosition.InsideBottomCenter => Alignment.LowerCenter,
            LegendPosition.InsideLeftCenter => Alignment.MiddleLeft,
            LegendPosition.InsideRightCenter => Alignment.MiddleRight,
            _ => Alignment.UpperLeft,
        };
    }

    #region Events
    private void ChartControl_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (_chartModel.HasEqualDimensions)
        {
            container.Width = Math.Min(Bounds.Width, Bounds.Height);
            container.Height = Math.Min(Bounds.Width, Bounds.Height);
        }
    }

    private void ScottPlot_PointerPressed_PointSelection(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var pointer = e.GetCurrentPoint((Avalonia.Visual?)sender);
        if (pointer.Properties.IsLeftButtonPressed)
        {
            var plotCoords = _plot.GetCoordinates((float)pointer.Position.X, (float)pointer.Position.Y);
            var dataPoint = GetNearestPoint(plotCoords, out int IndexInCurve, out Guid CurveId);
            if (!dataPoint.Equals(DataPoint.None))
            {
                CreateOrUpdateSelectedPointCrosshair(dataPoint.X, dataPoint.Y);
            }
            else
            {
                RemoveSelectedPointCrosshair();
            }

            ChartPointSelected?.Invoke(this, new ChartPointSelectedEventArgs(dataPoint.X, dataPoint.Y, IndexInCurve, CurveId));
        }

        scottPlot.Refresh();
    }

    private void CreateOrUpdateSelectedPointCrosshair(double x, double y)
    {
        if (_selectedPointCrosshair == null)
        {
            _selectedPointCrosshair = _plot.Add.Crosshair(x, y);
            _selectedPointCrosshair.LineColor = Color.FromColor(System.Drawing.Color.Red);
            _selectedPointCrosshair.LineWidth = 0.7f;
            _selectedPointCrosshair.LinePattern = LinePattern.DenselyDashed;
        }
        else
        {
            _selectedPointCrosshair.X = x;
            _selectedPointCrosshair.Y = y;
        }
    }

    private void RemoveSelectedPointCrosshair()
    {
        _plot.Remove(_selectedPointCrosshair);
        _selectedPointCrosshair = null;
    }

    private DataPoint GetNearestPoint(Coordinates coordinates, out int Index, out Guid CurveId)
    {
        var plottables = _plot.GetPlottables<Scatter>();

        foreach (var plottable in plottables.Where(x => x.IsVisible))
        {
            var point = plottable.GetNearest(coordinates, _plot.LastRender, 5);
            if (!point.Equals(DataPoint.None))
            {
                var id = _plottedCurves.FirstOrDefault(x => x.Value == plottable).Key;
                CurveId = id;
                var curve = _chartModel.GetCurveById(id);
                Index = curve.GetPointIndex(point.X, point.Y);
                return point;
            }
        }

        Index = -1;
        CurveId = new Guid();
        return DataPoint.None;
    }

    private void btnZoomOut_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _plot.Axes.ZoomOut(1.2, 1.2);
        scottPlot.Refresh();
    }

    private void btnZoomIn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _plot.Axes.ZoomIn(1.2, 1.2);
        scottPlot.Refresh();
    }

    private void btnResetZoom_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _plot.Axes.AutoScale();
        scottPlot.Refresh();
    }

    private void btnRectangleZoom_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        //bool hasZoomRectangle = scottPlot.UserInputProcessor.UserActionResponses.OfType<ScottPlot.Interactivity.UserActionResponses.MouseDragZoomRectangle>().Any()
        if ((bool)btnRectangleZoom.IsChecked)
        {
            scottPlot.UserInputProcessor.UserActionResponses.Add(new ScottPlot.Interactivity.UserActionResponses.MouseDragZoomRectangle(ScottPlot.Interactivity.StandardMouseButtons.Left));
        }
        else
        {
            scottPlot.UserInputProcessor.RemoveAll<ScottPlot.Interactivity.UserActionResponses.MouseDragZoomRectangle>();
        }
    }

    private void btnLegend_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ToggleLegendVisibility((bool)btnLegend.IsChecked);
    }

    private void ToggleLegendVisibility(bool visible)
    {
        if (visible)
        {
            _plot.ShowLegend();
            _chartModel.Legend.IsVisible = true;
        }
        else
        {
            _plot.HideLegend();
            _chartModel.Legend.IsVisible = false;
        }

        scottPlot.Refresh();
    }

    #endregion

    #region Public Methods
    public void ClearPlot(bool reBuild = true)
    {
        if (reBuild)
            scottPlot.Reset();
        else
            _plot.Clear();
    }

    public void SetAxisLimits(double minX, double minY, double maxX, double maxY)
    {
        _plot.Axes.SetLimitsX(minX, maxX);
        _plot.Axes.SetLimitsY(minY, maxY);
        scottPlot.Plot.Axes.TightMargins();

        scottPlot.Refresh();
    }

    public void EnableCrosshairPointSelection()
    {
        DisableCrosshairPointSelection();

        scottPlot.PointerPressed += ScottPlot_PointerPressed_PointSelection;
    }

    public void DisableCrosshairPointSelection()
    {
        scottPlot.PointerPressed -= ScottPlot_PointerPressed_PointSelection;
        RemoveSelectedPointCrosshair();
    }

    public void SetCurveVisibilty(Guid curveID, bool visible, bool resetZoom = false)
    {
        var plottable = _plottedCurves[curveID];
        var curve = _chartModel.GetCurveById(curveID);
        if (plottable == null || curve == null) return;
        plottable.IsVisible = visible;
        curve.IsVisible = visible;
        if (resetZoom) _plot.Axes.AutoScale();
        scottPlot.Refresh();
    }

    public void SelectPoint(Guid curveId, int pointIndex)
    {
        var curve = _chartModel.GetCurveById(curveId);

        if (pointIndex < 0 || curveId == Guid.Empty || curve == null || !curve.IsVisible || pointIndex >= curve.DataCount)
        {
            RemoveSelectedPointCrosshair();
        }
        else
        {
            CreateOrUpdateSelectedPointCrosshair(curve.Xvals[pointIndex], curve.Yvals[pointIndex]);
        }

        scottPlot.Refresh();
    }

    public Bitmap? GetMarkerGraphic(Guid curveID)
    {
        var curve = _chartModel.GetCurveById(curveID);
        if (curve == null) return null;
        int size = 12;
        return curve.MarkerType switch
        {
            MarkerType.FilledCircle => GraphicsGenerator.GetCircle(size, curve.MarkerColor),
            MarkerType.FilledSquare => GraphicsGenerator.GetSquare(size, curve.MarkerColor),
            MarkerType.FilledTriangleUp => GraphicsGenerator.GetTriangleUp(size, curve.MarkerColor),
            MarkerType.FilledTriangleDown => GraphicsGenerator.GetTriangleDown(size, curve.MarkerColor),
            MarkerType.FilledDiamond => GraphicsGenerator.GetDiamond(size, curve.MarkerColor),
            _ => null
        };
    }

    public void SetScrollZooming(bool enabled)
    {
        scottPlot.UserInputProcessor.RemoveAll<ScottPlot.Interactivity.UserActionResponses.MouseWheelZoom>();
        if (enabled)
        {
            scottPlot.UserInputProcessor.UserActionResponses.Add(new ScottPlot.Interactivity.UserActionResponses.MouseWheelZoom(ScottPlot.Interactivity.StandardKeys.Shift, ScottPlot.Interactivity.StandardKeys.Control) { ZoomFraction = 0.15 });
        }
    }
    #endregion
}