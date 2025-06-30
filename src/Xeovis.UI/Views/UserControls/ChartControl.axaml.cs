using Avalonia.Controls;
using ScottPlot;
using System;
using System.Collections.Generic;
using Xeovis.UI.ViewModels;
using ScottPlot.Plottables;
using System.Linq;
using Avalonia.Media.Imaging;
using Xeovis.Presentation;
using Avalonia;
using System.IO;
using Avalonia.Input;
using System.Text;
using Avalonia.Platform.Storage;
using Serilog;
using Xeovis.UI.Views.Helpers;
using Xeovis.UI.Models.Enums;
using Xeovis.Base;

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
    private bool _isPointSelectionActive;

    public event EventHandler<ChartPointSelectedEventArgs>? ChartPointSelected;

    public ChartControl()
    {
        InitializeComponent();
        SizeChanged += ChartControl_SizeChanged;

        scottPlot.UserInputProcessor.LeftClickDragPan(false);
        scottPlot.UserInputProcessor.DoubleLeftClickBenchmark(false);
        scottPlot.UserInputProcessor.RightClickDragZoom(false);
        scottPlot.UserInputProcessor.RemoveAll<ScottPlot.Interactivity.UserActionResponses.MouseDragZoomRectangle>();

        scottPlot.Menu = null;
        scottPlot.KeyDown += scottPlot_KeyDown;
        scottPlot.PointerPressed += scottPlot_PointerPressed;
        
        btnRectangleZoom.IsCheckedChanged += btnRectangleZoom_IsCheckedChanged;
        btnResetZoom.Click += btnResetZoom_Click;
        btnZoomIn.Click += btnZoomIn_Click;
        btnZoomOut.Click += btnZoomOut_Click;
        btnLegend.IsCheckedChanged += btnLegend_IsCheckedChanged;
    }

    private void scottPlot_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint((Visual?)sender);
        var plotX = point.Position.X - scottPlot.Plot.LastRender.DataRect.Left;
        var plotY = point.Position.Y - scottPlot.Plot.LastRender.DataRect.Top;
        if (point.Properties.IsRightButtonPressed)
        {
            SetContextMenu(point);
        }
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
                Log.Error("Couldn't plot {type} curve: {name}", curve.CurveType, curve.Name);
        }

        SetProperties();

        if (resetZoom) _plot.Axes.AutoScale();

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

    private void SetContextMenu(PointerPoint pointer)
    {
        var menu = new ContextMenu();

        /// Zoom Items
        var zoomItem = new MenuItem() { Header = "Zoom" };
        var zoomInItem = new MenuItem() { Header = "Zoom In" };
        zoomInItem.Click += (s, e) => ZoomIn();
        zoomItem.Items.Add(zoomInItem);

        var zoomOutItem = new MenuItem() { Header = "Zoom Out" };
        zoomOutItem.Click += (s, e) => ZoomOut();
        zoomItem.Items.Add(zoomOutItem);

        zoomItem.Items.Add(new Separator());

        var zoomResetItem = new MenuItem() { Header = "Reset Zoom" };
        zoomResetItem.Click += (s, e) => ResetZoom();
        zoomItem.Items.Add(zoomResetItem);

        menu.Items.Add(zoomItem);

        /// Export Data
        var exportDataItem = new MenuItem() { Header = "Export Data" };
        var copyDataToClipBoard = new MenuItem() { Header = "Copy to clipboard" };
        copyDataToClipBoard.Click += (s, e) => TopLevel.GetTopLevel(this)?.Clipboard?.SetTextAsync(GetDataString());
        exportDataItem.Items.Add(copyDataToClipBoard);

        var exportAsTxt = new MenuItem() { Header = "Export as Text" };
        exportAsTxt.Click += (s, e) => SaveDataAsTxt();
        exportDataItem.Items.Add(exportAsTxt);

        menu.Items.Add(exportDataItem);

        /// Export Image
        var exportImageItem = new MenuItem() { Header = "Export Image" };
        var exportAsPNG = new MenuItem() { Header = "Export as PNG" };
        exportAsPNG.Click += (s, e) => SaveChartImageAsPNG(FileExtensionType.PNG);
        exportImageItem.Items.Add(exportAsPNG);

        var exportAsJPEG = new MenuItem() { Header = "Export as JPEG" };
        exportAsJPEG.Click += (s, e) => SaveChartImageAsPNG(FileExtensionType.JPEG);
        exportImageItem.Items.Add(exportAsJPEG);

        menu.Items.Add(exportImageItem);

        /// Chart settings
        menu.Items.Add(new Separator());

        var showLegendItem = new MenuItem() { Header = "Show Legend", ToggleType = MenuItemToggleType.CheckBox, IsChecked = scottPlot.Plot.Legend.IsVisible };
        showLegendItem.Click += (s, e) => ToggleLegendVisibility(showLegendItem.IsChecked);
        menu.Items.Add(showLegendItem);

        var showGridItem = new MenuItem() { Header = "Show Grid Lines", ToggleType = MenuItemToggleType.CheckBox, IsChecked = scottPlot.Plot.Grid.IsVisible };
        showGridItem.Click += (s, e) => ToggleGridLinesVisibility(showGridItem.IsChecked);
        menu.Items.Add(showGridItem);

        /// other
        menu.Items.Add(new Separator());

        var copyCoordinates = new MenuItem() { Header = "Copy Coordinates" };
        copyCoordinates.Click += (s, e) =>
        {
            var coords = scottPlot.Plot.GetCoordinates((float)pointer.Position.X, (float)pointer.Position.Y);
            TopLevel.GetTopLevel(this)?.Clipboard?.SetTextAsync($"{coords.X.Round(8)}\t{coords.Y.Round(8)}");
        };

        menu.Items.Add(copyCoordinates);

        scottPlot.ContextMenu = menu;
        scottPlot.ContextMenu.Open();
    }

    public async void SaveChartImageAsPNG(FileExtensionType extensionType)
    {
        // Ensure layout is up to date
        //control.Measure(Avalonia.Size.Infinity);
        //control.Arrange(new Rect(control.DesiredSize));
        //control.UpdateLayout();

        var options = new FilePickerSaveOptions();
        options.Title = "Save PNG File";
        options.FileTypeChoices = [FileExtentionHelper.GetFilePickerFileType(extensionType)];
        var file = await TopLevel.GetTopLevel(this).StorageProvider.SaveFilePickerAsync(options);

        if (file != null)
        {
            var bitmapRender = GetChartBitmapRender();
            bitmapRender.Save(file.Path.LocalPath);
        }
    }

    private RenderTargetBitmap GetChartBitmapRender()
    {
        var control = scottPlot;
        var dpi = new Vector(300, 300); // standard DPI
        var scale = 300.0 / 96.0; // 96 is Avalonia's default DPI
        var width = (int)(control.Bounds.Width * scale);
        var height = (int)(control.Bounds.Height * scale);
        var pixelSize = new Avalonia.PixelSize(width, height);
        var bitmapRender = new RenderTargetBitmap(pixelSize, dpi);
        bitmapRender.Render(control);
        return bitmapRender;
    }

    public Avalonia.Controls.Image GetChartImage()
    {
        var bitmapRender = GetChartBitmapRender();
        var image = new Avalonia.Controls.Image();
        image.Source = bitmapRender;
        return image;
    }

    public string GetDataString()
    {
        int maxCurveSpace = _chartModel.Curves.Max(x => x.Name.Length) + 5;
        var sb = new StringBuilder();
        var title = $" {_viewModel.Title} ";
        sb.AppendLine($"{title.PadLeft(maxCurveSpace, '-').PadRight(maxCurveSpace * 2, '-')}");
        sb.AppendLine($"{"Name".PadRight(maxCurveSpace)}{"X".PadRight(10)}Y");

        var visibleCurves = _chartModel.Curves.Where(c => c.IsVisible && c.IsVisibleInLegend).ToList();
        if (visibleCurves.Count == 0) return string.Empty;
        foreach (var curve in visibleCurves)
        {
            for (int i = 0; i < curve.DataCount; i++)
                sb.AppendLine(($"{curve.Name.PadRight(maxCurveSpace)}{curve.Xvals[i].ToString().PadRight(10)}{curve.Yvals[i]}"));
        }
        
        return sb.ToString();
    }

    private async void SaveDataAsTxt()
    {
        var options = new FilePickerSaveOptions();
        options.Title = "Save Text File";
        options.FileTypeChoices = [FileExtentionHelper.GetFilePickerFileType(Models.Enums.FileExtensionType.Txt)];
        var file = await TopLevel.GetTopLevel(this).StorageProvider.SaveFilePickerAsync(options);

        if (file != null)
        {
            using var stream = await file.OpenWriteAsync();
            using var streamWriter = new StreamWriter(stream);
            await streamWriter.WriteLineAsync(GetDataString());
        }
    }

    private void ChartControl_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (_chartModel.HasEqualDimensions)
        {
            container.Width = Math.Min(Bounds.Width, Bounds.Height);
            container.Height = Math.Min(Bounds.Width, Bounds.Height);
        }
    }

    private void ScottPlot_PointerPressed_PointSelection(object? sender, PointerPressedEventArgs e)
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
        ZoomOut();
    }

    private void ZoomOut()
    {
        _plot.Axes.ZoomOut(1.2, 1.2);
        scottPlot.Refresh();
    }

    private void btnZoomIn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ZoomIn();
    }

    private void ZoomIn()
    {
        _plot.Axes.ZoomIn(1.2, 1.2);
        scottPlot.Refresh();
    }

    private void btnResetZoom_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ResetZoom();
    }

    private void btnRectangleZoom_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        //bool hasZoomRectangle = scottPlot.UserInputProcessor.UserActionResponses.OfType<ScottPlot.Interactivity.UserActionResponses.MouseDragZoomRectangle>().Any()
        if ((bool)btnRectangleZoom.IsChecked)
        {
            scottPlot.UserInputProcessor.UserActionResponses.Add(new ScottPlot.Interactivity.UserActionResponses.MouseDragZoomRectangle(ScottPlot.Interactivity.StandardMouseButtons.Left));
            if (_isPointSelectionActive)
                scottPlot.PointerPressed -= ScottPlot_PointerPressed_PointSelection;
        }
        else
        {
            scottPlot.UserInputProcessor.RemoveAll<ScottPlot.Interactivity.UserActionResponses.MouseDragZoomRectangle>();
            if (_isPointSelectionActive)
                scottPlot.PointerPressed += ScottPlot_PointerPressed_PointSelection;
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
            btnLegend.IsChecked = true;
        }
        else
        {
            _plot.HideLegend();
            _chartModel.Legend.IsVisible = false;
            btnLegend.IsChecked = false;
        }

        scottPlot.Refresh();
    }

    private void ToggleGridLinesVisibility(bool visible)
    {
        if (visible)
        {
            _plot.Grid.IsVisible = true;
            _chartModel.IsGridVisible = true;
        }
        else
        {
            _plot.Grid.IsVisible = false;
            _chartModel.IsGridVisible = false;
        }

        scottPlot.Refresh();
    }

    private void scottPlot_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Avalonia.Input.Key.Escape)
        {
            if (_isPointSelectionActive)
            {
                RemoveSelectedPointCrosshair();
                ChartPointSelected?.Invoke(this, null);
                scottPlot.Refresh();
            }
        }
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

    public void ResetZoom()
    {
        _plot.Axes.AutoScale();
        scottPlot.Refresh();
    }

    public void SetAxisLimits(double minX, double minY, double maxX, double maxY)
    {
        _plot.Axes.SetLimitsX(minX, maxX);
        _plot.Axes.SetLimitsY(minY, maxY);
        //_plot.Axes.TightMargins();

        scottPlot.Refresh();
    }

    public void EnableCrosshairPointSelection()
    {
        DisableCrosshairPointSelection();
        _isPointSelectionActive = true;
        scottPlot.PointerPressed += ScottPlot_PointerPressed_PointSelection;
    }

    public void DisableCrosshairPointSelection()
    {
        scottPlot.PointerPressed -= ScottPlot_PointerPressed_PointSelection;
        _isPointSelectionActive = false;
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
        const int size = 12;
        var graphic = curve.MarkerType switch
        {
            MarkerType.FilledCircle => GraphicsGenerator.GetCircle(size, curve.MarkerColor),
            MarkerType.FilledSquare => GraphicsGenerator.GetSquare(size, curve.MarkerColor),
            MarkerType.FilledTriangleUp => GraphicsGenerator.GetTriangleUp(size, curve.MarkerColor),
            MarkerType.FilledTriangleDown => GraphicsGenerator.GetTriangleDown(size, curve.MarkerColor),
            MarkerType.FilledDiamond => GraphicsGenerator.GetDiamond(size, curve.MarkerColor),
            _ => null
        };
        
        if (graphic == null) Log.Warning("Not supported curve marker type: {type}", curve.MarkerType);
        return graphic;
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