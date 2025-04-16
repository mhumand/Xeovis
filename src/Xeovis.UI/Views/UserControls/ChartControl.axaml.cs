using Avalonia.Controls;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.ViewModels;

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

    public ChartControl()
    {
        InitializeComponent();
        scottPlot.SizeChanged += ScottPlot_SizeChanged;
        scottPlot.PointerPressed += ScottPlot_PointerPressed;
    }

    public void ClearPlot(bool reBuild = true)
    {
        if (reBuild)
            scottPlot.Reset();
        else
            scottPlot.Plot.Clear();
    }

    public bool Plot(ChartModel chartModel, bool reBuild = true, bool resetZoom = true)
    {
        if (chartModel == null) return false;
        _viewModel = new ChartControlViewModel(chartModel);

        ClearPlot(reBuild);

        foreach (var curve in _chartModel.Curves)
        {
            if (curve.IsHidden || curve.DataCount == 0) continue;

            bool successful = curve.CurveType switch
            {
                CurveType.Scatter => AddScatter(curve),
                CurveType.Line => AddLine(curve),
                CurveType.CrossHair => AddCrossHair(curve),
                _ => false,
            };

            if (!successful)
            {

            }
        }

        SetProperties();
        if (resetZoom) scottPlot.Plot.Axes.AutoScale();
        scottPlot.Refresh();
        return true;
    }

    private void SetProperties()
    {
        scottPlot.Plot.Axes.Left.Label.Text = _chartModel.XAxisLabel;
        scottPlot.Plot.Axes.Bottom.Label.Text = _chartModel.YAxisLabel;
        
        scottPlot.Plot.Axes.Title.Label.Text = _chartModel.Title;
        scottPlot.Plot.Axes.Title.IsVisible = _chartModel.IsTitleVisible;

        scottPlot.Plot.Legend.IsVisible = _chartModel.Legend.IsVisible;
        scottPlot.Plot.Legend.Alignment = GetScottPlotLegendAlignment(_chartModel.Legend.Position);
        scottPlot.Plot.Legend.BackgroundColor = Color.FromColor(System.Drawing.Color.Transparent);
        scottPlot.Plot.Legend.ShadowColor = Color.FromColor(System.Drawing.Color.Transparent);
        scottPlot.Plot.Legend.OutlineWidth = 0.5f;

        scottPlot.Plot.Axes.SquareUnits(_chartModel.HasEqualDimensions);
    }

    private bool AddScatter(CurveModel curve)
    {
        var x = curve.Xvals.ToArray();
        var y = curve.Yvals.ToArray();

        var scatter = scottPlot.Plot.Add.Scatter(x, y);
        scatter.LineStyle.IsVisible = curve.IsLineVisible;
        scatter.LineColor = Color.FromColor(curve.LineColor);
        scatter.LineStyle.Width = curve.LineWidth;
        scatter.LineStyle.Pattern = GetScottPlotLinePattern(curve.LineType);
        scatter.LineStyle.AntiAlias = curve.IsLineAntiAlis;

        scatter.MarkerStyle.Size = curve.MarkerSize;
        scatter.MarkerColor = Color.FromColor(curve.MarkerColor);
        //scatter.MarkerStyle.OutlineStyle.Color = Color.FromColor(curve.MarkerColor);
        scatter.MarkerStyle.IsVisible = curve.IsMarkerVisible;
        scatter.MarkerStyle.Shape = GetScottPlotMarkerShape(curve.MarkerType);
        scatter.LegendText = curve.Name;

        _plottedCurves.Add(curve.Id, scatter);

        return true;
    }

    private bool AddLine(CurveModel curve)
    {
        return true;
    }

    private bool AddCrossHair(CurveModel curve)
    {
        return true;
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
    private void ScottPlot_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (_chartModel.HasEqualDimensions)
            scottPlot.Width = e.NewSize.Height;
    }

    private void ScottPlot_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var hitPoint = e.GetPosition(scottPlot);
        var plotCoords = scottPlot.Plot.GetCoordinates((float)hitPoint.X, (float)hitPoint.Y);
        foreach (Scatter plottable in scottPlot.Plot.GetPlottables<Scatter>())
        {
            var b = plottable.GetNearest(plotCoords, scottPlot.Plot.LastRender, 10);
        }

        var bounds = scottPlot.Plot.Legend.GetLayout(new PixelSize(1,1));
    }
    #endregion
}