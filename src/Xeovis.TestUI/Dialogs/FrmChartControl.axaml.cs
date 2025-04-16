using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
using System.Collections.Generic;
using System.Drawing;
using Xeovis.UI.Models;

namespace Xeovis.TestUI;

public partial class FrmChartControl : Window
{
    public FrmChartControl()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void btnPlot1_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var curve1 = new CurveModel();
        curve1.Name = "Curve 1";
        curve1.Xvals = new List<double>() { 1, 2, 3, 3.2, 5, 5.5, 7 };
        curve1.Yvals = new List<double>() { 1.5, 2.7, 3, 3.5, 6.2, 6.8, 7.7 };

        var curve2 = new CurveModel();
        curve2.Name = "Curve 2";
        curve2.Xvals = new List<double>() { 1, 2, 3, 3.2, 5, 5.5, 7 };
        curve2.Yvals = new List<double>() { 0.8, 1.5, 3.2, 5.1, 5.4, 6.5 };

        curve2.IsLineVisible = false;
        curve2.MarkerType = UI.Models.Enums.MarkerType.OpenCircle;
        curve2.MarkerColor = Color.Blue;

        var chartModel = new ChartModel();
        chartModel.Title = "Chart 1";
        chartModel.Curves.Add(curve1);
        chartModel.Curves.Add(curve2);

        chartControl1.Plot(chartModel);
    }

    private void btnPlot2_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var curve1 = new CurveModel();
        curve1.Name = "Curve 1";
        curve1.Xvals = new List<double>() { 0, 1 };
        curve1.Yvals = new List<double>() { 0, 1 };
        curve1.CurveType = UI.Models.Enums.CurveType.Line;
        curve1.IsMarkerVisible = false;
        curve1.LineWidth = 1;

        var cross1 = new CurveModel();
        cross1.Name = "Crosshair 1";
        cross1.Xvals = new List<double>() { 0.7};
        cross1.Yvals = new List<double>() { 0.2};
        cross1.LineColor = Color.Orange;
        cross1.CurveType = UI.Models.Enums.CurveType.Crosshair;
        cross1.LineWidth = 1;
        cross1.IsMarkerVisible = false;

        var curve2 = new CurveModel();
        curve2.Name = "Curve 2";
        curve2.Xvals = new List<double>() { 0.1, 0.2, 0.3, 0.4, 0.5, 0.7, 0.9, 1.0 };
        curve2.Yvals = new List<double>() { 0.15, 0.18, 0.25, 0.39, 0.51, 0.75, 0.88, 0.9 };
        curve2.IsLineVisible = false;
        curve2.MarkerType = UI.Models.Enums.MarkerType.OpenCircle;
        curve2.MarkerColor = Color.Blue;

        var chartModel = new ChartModel();
        chartModel.Title = "Chart 2 - Equal Dimensions";
        chartModel.Curves.Add(curve1);
        chartModel.Curves.Add(curve2);
        chartModel.Curves.Add(cross1);
        chartModel.HasEqualDimensions = true;
        chartModel.IsGridVisible = false;

        chartControl2.Plot(chartModel);
    }
}