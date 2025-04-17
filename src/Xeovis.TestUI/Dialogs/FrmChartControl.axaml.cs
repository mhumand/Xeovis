using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xeovis.UI;
using Xeovis.UI.Models;

namespace Xeovis.TestUI;

public partial class FrmChartControl : Window
{
    public FrmChartControl()
    {
        InitializeComponent();
        DataContext = this;
        btnToggleVisibility.IsCheckedChanged += btnToggleVisibility_IsCheckedChanged;
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
        cross1.Xvals = new List<double>() { 0.47};
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
        chartModel.XAxis = new AxisModel("Label for X Axis", 0, 1);
        chartModel.YAxis = new AxisModel("Label for Y Axis", 0, 1);

        chartControl2.Plot(chartModel);
        chartControl2.SetAxisLimits(0, 0, 1, 1);
        chartControl2.EnableCrosshairPointSelection();
        chartControl2.ChartPointSelected += ChartControl2_ChartPointSelected;
    }

    private void ChartControl2_ChartPointSelected(object? sender, UI.Models.Events.ChartPointSelectedEventArgs e)
    {
        if (sender is not ChartControl chart) return;

        if (e.HasValidCurveId && e.HasValidPoint)
            txt2.Text = $"User selected point ({e.X},{e.Y}) from curve '{chart.ViewModel.ChartModel.GerCurveById(e.CurveId)}'";
        else
            txt2.Text = string.Empty;
    }


    private void btnToggleVisibility_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (chartControl2.ViewModel.ChartModel.Curves.Any())
            chartControl2.SetCurveVisibilty(chartControl2.ViewModel.ChartModel.Curves[0].Id, (bool)btnToggleVisibility.IsChecked);
    }

    private void btnSelectPoint_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (chartControl2.ViewModel.ChartModel.Curves.Any())
        {
            var curve = chartControl2.ViewModel.ChartModel.Curves[1];
            chartControl2.SelectPoint(curve.Id, new Random().Next(curve.DataCount - 1));
        }
    }
}