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
        chartModel.Title = "Chart 2 - Equal Dimensions";
        chartModel.Curves.Add(curve1);
        chartModel.Curves.Add(curve2);
        chartModel.HasEqualDimensions = true;

        chartControl2.Plot(chartModel);
    }
}