using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xeovis.Base;
using Xeovis.Base.Comparers;
using Xeovis.Domain;
using Xeovis.Presentation;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.ViewModels;
using Xeovis.UI.ViewModels.DialogsViewModels;
using Xeovis.UI.Views;

namespace Xeovis.UI;

public partial class mctcPlotGridViewTabControl : UserControl, ImctcPlotView
{
    public Guid Id { get; set; }
    public mctcPlotViewType ViewType { get; set; }

    private mctcPlotGridViewModel _viewModel = new mctcPlotGridViewModel();
    public mctcPlotGridViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new mctcPlotGridViewModel();
            DataContext = _viewModel;
            PrepareUI();
        }
    }

    public MainWindow? ParentWindow { get; set; }

    public Rect InitialBounds { get; set; }

    public mctcPlotGridViewTabControl()
    {
        InitializeComponent();
        if (this.Content is Control content)
            content.DataContext = this.DataContext;
    }

    private void PrepareUI()
    {
        uniformGrid.SizeChanged += (s, e) => SetupLayout((int)spinnerColumnNo.ViewModel.Value);
        
        var vmSpinnerMctcRun = new LabelSpinnerControlViewModel("mctc Run No.", 1, 1, 1);
        spinnerMctcRun.ViewModel = vmSpinnerMctcRun;
        spinnerMctcRun.ValueChanged += spinnerMctcRun_ValueChanged;

        var vmSpinnerColumnNo = new LabelSpinnerControlViewModel("Column No.", 1, 1, 1);
        spinnerColumnNo.ViewModel = vmSpinnerColumnNo;
        spinnerColumnNo.ValueChanged += spinnerColumnNo_ValueChanged;

        slider.ValueChanged += slider_ValueChanged;
        slider.Minimum = uniformGrid.Bounds.Width;
        slider.Maximum = 10000;
        slider.Value = uniformGrid.Bounds.Width;
        tableFilter.CellValueChanged += tableFilter_CellValueChanged;

        var vmYAxisType = new LabelComboBoxControlViewModel();
        vmYAxisType.Label = "Type";
        vmYAxisType.IsStackedHorizontally = true;
        lcbcYAxisType.ViewModel = vmYAxisType;
        lcbcYAxisType.ListSelectionChanged += lcbcYAxisType_ListSelectionChanged;
        lcbcYAxisType.SetItems((new List<ListableItem>()
        {
            new ListableItem(mctcPlotAxisType.Calculated),
            new ListableItem(mctcPlotAxisType.Residual),
        }));

        var vmYAxisVariable = new LabelComboBoxControlViewModel();
        vmYAxisVariable.Label = "Variable";
        vmYAxisVariable.IsStackedHorizontally = true;
        vmYAxisVariable.IsReadOnly = true;
        lcbcYAxisVariable.ViewModel = vmYAxisVariable;

        var vmXAxisType = new LabelComboBoxControlViewModel();
        vmXAxisType.Label = "Type";
        vmXAxisType.IsStackedHorizontally = true;
        lcbcXAxisType.ViewModel = vmXAxisType;
        lcbcXAxisType.ListSelectionChanged += lcbcXAxisType_ListSelectionChanged;
        lcbcXAxisType.SetItems(new List<ListableItem>()
        {
            new ListableItem(mctcPlotAxisType.Observed),
            new ListableItem(mctcPlotAxisType.Temperature),
            new ListableItem(mctcPlotAxisType.Pressure),
        });

        var vmXAxisVariable = new LabelComboBoxControlViewModel();
        vmXAxisVariable.Label = "Variable";
        vmXAxisVariable.IsStackedHorizontally = true;
        lcbcXAxisVariable.ViewModel = vmXAxisVariable;
        lcbcXAxisVariable.ListSelectionChanged += lcbcXAxisVariable_ListSelectionChanged;
    }

    public bool PopulateUI(mctcPlotDto mctcPlotDto, mctcBundle mctcBundle)
    {
        if (mctcPlotDto == null || mctcBundle == null) return false;

        _viewModel.mctcPlotDto = mctcPlotDto;
        _viewModel.mctcBundle = mctcBundle;

        spinnerMctcRun.ViewModel.MaxValue = mctcBundle.mctcVariables[0].Values.Count;
        spinnerMctcRun.ValueChanged -= spinnerMctcRun_ValueChanged;
        spinnerMctcRun.ViewModel.Value = 1;
        _viewModel.mctcRunIndex = 0;
        spinnerMctcRun.ValueChanged += spinnerMctcRun_ValueChanged;

        spinnerColumnNo.ViewModel.MaxValue = mctcPlotDto.SampleXeosVariables.Count;

        _viewModel.ClearInUseMarkersAndColors();
        PopulateMctcRunsTable();
        PopulatePhaseFilters();
        lcbcXAxisVariable.SetItems(_viewModel.GetXeosVarItems());
        PopulateGridCharts();

        if (!((int)spinnerColumnNo.ViewModel.Value).IsBetween(4, mctcPlotDto.SampleXeosVariables.Count))
            spinnerColumnNo.ViewModel.Value = Math.Min(4, mctcPlotDto.SampleXeosVariables.Count);

        return true;
    }

    public void Refresh()
    {
        PopulateUI(_viewModel.mctcPlotDto, _viewModel.mctcBundle);
    }

    private void tableFilter_CellValueChanged(object? sender, TableCellValueChangedEventArgs e)
    {
        var column = tableFilter.ViewModel.TableModel.GetColumnById(e.ColumnId);
        if (column != null && column.HasOptions && !column.IsReadOnly)
        {
            var key = _viewModel.PhaseFilters.Keys.ToList()[e.RowIndex];
            _viewModel.PhaseFilters[key] = EnumExtensions.GetValueFromDescription<FilterType>(e.Value.ToString());
            PopulateGridCharts();
        }
    }

    private void btnClearFilters_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        PopulatePhaseFilters();
        PopulateGridCharts();
    }

    private void PopulateGridCharts()
    {
        if (lcbcYAxisType.ViewModel.SelectedItem is not ListableItem yAxisType) return;
        if (lcbcXAxisType.ViewModel.SelectedItem is not ListableItem xAxisType) return;
        if (yAxisType.Tag is not mctcPlotAxisType YAxisType) return;
        if (xAxisType.Tag is not mctcPlotAxisType XAxisType) return;

        if (lcbcXAxisVariable.ViewModel.SelectedItem is not ListableItem xVarItem) return;

        var XAxisVariable = (xeosVariable)xVarItem.Tag;
        bool selfComparison = XAxisVariable == null;

        var chartControls = new List<ChartControl>();
        foreach (var sampleVar in _viewModel.mctcPlotDto.SampleXeosVariables)
        {
            var viewModel = new ChartControlViewModel();
            var chartControl = new ChartControl();
            chartControl.ViewModel = viewModel;
            chartControl.SetScrollZooming(false);

            var xVar = selfComparison ? sampleVar : XAxisVariable;
            var axesSettings = new mctcPlotAxesSettings(sampleVar, xVar, YAxisType, XAxisType);
            var chartModel = _viewModel.CreateChartModel(axesSettings);
            var successful = chartControl.PopulateChart(chartModel);
            if (!successful) continue;
            chartControls.Add(chartControl);
        }

        uniformGrid.Children.Clear();
        uniformGrid.Columns = (int)spinnerColumnNo.ViewModel.Value;
        uniformGrid.Rows = chartControls.Count / uniformGrid.Columns + 1;
        chartControls.ForEach(x => uniformGrid.Children.Add(x));

        PopulateLegend();
    }

    private void PopulateLegend()
    {
        stackLegend.Children.Clear();
        var checkboxes = new List<CheckBoxControl>();
        var curveNames = new List<string>();
        foreach (ChartControl chartControl in uniformGrid.Children)
        {
            if (chartControl == null) continue;
            foreach (var curve in chartControl.ViewModel.ChartModel.Curves)
            {
                if (curveNames.Contains(curve.Name)) continue;
                curveNames.Add(curve.Name);
                var markerImage = chartControl.GetMarkerGraphic(curve.Id);
                if (markerImage == null || !curve.IsVisibleInLegend) continue;
                var checkboxViewModel = new CheckBoxControlViewModel(curve.Name, isChecked: curve.IsVisible) { Image = markerImage };
                var checkboxControl = new CheckBoxControl() { ViewModel = checkboxViewModel, Tag = curve.Name };
                checkboxControl.CheckStateChanged += checkboxControl_CheckStateChanged; ;
                checkboxControl.PointerPressed += checkboxControl_PointerPressed; ;
                checkboxControl.ContextMenu = GetLegendCheckboxContextMenu();
                checkboxControl.Margin = new Avalonia.Thickness(5, 0);
                checkboxes.Add(checkboxControl);
            }
        }

        if (!_viewModel.mctcPlotDto.PointsGroupingType.IsNumber())
            stackLegend.Children.AddRange(checkboxes);
        else
            stackLegend.Children.AddRange(checkboxes.OrderBy(x => x.ViewModel.Label, new NumericStringComparer()));
    }

    private ContextMenu GetLegendCheckboxContextMenu()
    {
        var menu = new ContextMenu();

        var checkAllItem = new MenuItem() { Header = "Check All" };
        checkAllItem.Click += checkAllItem_Click;

        var uncheckAllItem = new MenuItem() { Header = "Uncheck All" };
        uncheckAllItem.Click += uncheckAllItem_Click;

        var checkThisOnly = new MenuItem() { Header = "Check This Only" };
        checkThisOnly.Click += (s, e) => CheckThisOnly((s as MenuItem).Parent.Parent.Parent as CheckBoxControl);

        menu.Items.Add(checkAllItem);
        menu.Items.Add(uncheckAllItem);
        menu.Items.Add(new Separator());
        menu.Items.Add(checkThisOnly);
        return menu;
    }

    private void uncheckAllItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        foreach (CheckBoxControl control in stackLegend.Children)
            control.ViewModel.IsChecked = false;
    }

    private void checkAllItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        foreach (CheckBoxControl control in stackLegend.Children)
            control.ViewModel.IsChecked = true;
    }

    private void checkboxControl_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(sender as Control);
        if (point.Properties.IsMiddleButtonPressed)
        {
            var checkbox = sender as CheckBoxControl;
            CheckThisOnly(checkbox);
        }
    }

    private void CheckThisOnly(CheckBoxControl checkboxControl)
    {
        if (checkboxControl == null) return;
        checkboxControl.ViewModel.IsChecked = true;
        foreach (var control in stackLegend.Children.Except(new List<Control>() { checkboxControl }))
            (control as CheckBoxControl).ViewModel.IsChecked = false;
    }

    private void checkboxControl_CheckStateChanged(object? sender, Models.Events.CheckStateChangedEventArgs e)
    {
        if (sender is not CheckBoxControl checkbox) return;
        foreach (ChartControl chartControl in uniformGrid.Children)
        {
            if (chartControl == null) continue;
            var curve = chartControl.ViewModel.ChartModel.Curves.FirstOrDefault(c => c.Name == checkbox.Tag.ToString());
            if (curve != null)
                chartControl.SetCurveVisibilty(curve.Id, e.IsChecked);
        }
    }

    private void PopulateMctcRunsTable()
    {
        var tableModel = _viewModel.CreateMctcRunsTableModel();
        tableMctcRuns.PopulateTable(tableModel);
    }

    private void PopulatePhaseFilters()
    {
        var tableModel = _viewModel.CreateFiltersTableModel();
        tableFilter.PopulateTable(tableModel);
    }

    private void btnSidePanel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        splitView.IsPaneOpen = !splitView.IsPaneOpen;
    }

    private void spinnerMctcRun_ValueChanged(object? sender, Models.Events.SpinnerValueChangedEventArgs e)
    {
        _viewModel.mctcRunIndex = e.Value - 1;
        PopulateMctcRunsTable();
        PopulateGridCharts();
    }

    private void slider_ValueChanged(object? sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
        uniformGrid.Height = slider.Value;
    }

    private void spinnerColumnNo_ValueChanged(object? sender, Models.Events.SpinnerValueChangedEventArgs e)
    {
        SetupLayout(e.Value);
    }

    private void SetupLayout(int columnNo)
    {
        uniformGrid.Columns = columnNo;
        uniformGrid.Rows = uniformGrid.Children.Count / columnNo + 1;

        var width = uniformGrid.Bounds.Width / uniformGrid.Columns;
        if (width == 0) width = (InitialBounds.Width - splitView.OpenPaneLength) / uniformGrid.Columns;
        uniformGrid.Height = uniformGrid.Rows * width;
        slider.Value = uniformGrid.Height;
    }

    private async void btnSettings_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (ParentWindow == null) return;
        var otherNames = ParentWindow.ViewModel.mctcPlotDtos.Except(new List<mctcPlotDto> { _viewModel.mctcPlotDto }).Select(x => x.Name).ToList();
        var viewModel = new mctcPlotSettingsViewModel(_viewModel.mctcPlotDto, otherNames);
        var dialog = new mctcPlotSettingsDialog();
        dialog.ViewModel = viewModel;
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);

        if (result == DialogResultType.OK)
        {
            PopulateUI(_viewModel.mctcPlotDto, _viewModel.mctcBundle);
            ParentWindow.OnMctcPlotRenamed(_viewModel.mctcPlotDto);
        }
    }

    private async void btnExport_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (ParentWindow == null) return;

        var dialog = new mctcPlotGridExportDialog(_viewModel.mctcPlotDto);
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);

        if (result != DialogResultType.OK) return;

        if (dialog.ExportData || dialog.ExportImages)
        {
            var directory = await GetExportDirectory();
            if (!Directory.Exists(directory)) return;
            if (dialog.ExportImages) ExportImages(dialog, directory);
            if (dialog.ExportData) ExportData(dialog, directory);
        }
    }

    private async Task<string> GetExportDirectory()
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return string.Empty;
        var options = new FolderPickerOpenOptions()
        {
            Title = "Choose Folder",
            AllowMultiple = false,
        };

        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(options);
        if (folders.Count > 0)
            return Uri.UnescapeDataString(folders[0].Path.AbsolutePath);
        else
            return string.Empty;
    }

    private void ExportImages(mctcPlotGridExportDialog dialog, string directory)
    {
        var imageFileExtension = dialog.ImageFileType.GetStringExtention();
        var imageFileName = directory + dialog.ImageFilesName + imageFileExtension;
        var images = new List<Image>();
        foreach (ChartControl chartControl in uniformGrid.Children)
        {
            var image = chartControl.GetChartImage();
            image.Tag = chartControl;
            images.Add(image);
        }

        var sampleImage = images[0].Source as RenderTargetBitmap;
        if (sampleImage == null) return;
        var singleWidth = sampleImage.PixelSize.Width;
        var singleHeight = sampleImage.PixelSize.Height;
        int totalWidth = singleWidth * uniformGrid.Columns;
        int totalHeight = singleHeight * uniformGrid.Rows;
        var combined = new RenderTargetBitmap(new PixelSize(totalWidth, totalHeight), sampleImage.Dpi);
        var scale = 300.0 / 96.0;
        using (var ctx = combined.CreateDrawingContext())
        {
            ctx.FillRectangle(Brushes.White, new Rect(0, 0, totalWidth, totalHeight)); // background

            int counter = 0;
            for (int i = 0; i < uniformGrid.Rows; i++)
            {
                for (int j = 0; j < uniformGrid.Columns; j++)
                {
                    if (counter >= images.Count) break;

                    var image = images[counter];
                    var imageSource = image.Source as RenderTargetBitmap;
                    var width = imageSource.PixelSize.Width;
                    var height = imageSource.PixelSize.Height;
                    var imageRec = new Rect(0, 0, width, height);
                    ctx.DrawImage(imageSource, imageRec, new Rect(width * j / scale, height * i / scale, width / scale, height / scale));

                    if (dialog.IndiviualExport)
                    {
                        var chartControl = image.Tag as ChartControl;
                        imageSource.Save(directory + dialog.ImageFilesName + $" - {chartControl.ViewModel.Title}" + imageFileExtension);
                    }

                    counter++;
                }
            }

            combined.Save(imageFileName);
        }
    }

    private void ExportData(mctcPlotGridExportDialog dialog, string directory)
    {
        var dataFileExtension = dialog.DataFileType.GetStringExtention();
        var dataSB = new StringBuilder();
        foreach (ChartControl chartControl in uniformGrid.Children)
        {
            var dataString = chartControl.GetDataString();
            if (string.IsNullOrWhiteSpace(dataString)) continue;
            dataSB.AppendLine(dataString);

            if (dialog.IndiviualExport)
                File.WriteAllText(directory + dialog.DataFilesName + $" - {chartControl.ViewModel.Title}" + dataFileExtension, dataString);
        }

        if (dataSB.Length > 0)
            File.WriteAllText(directory + dialog.DataFilesName + dataFileExtension, dataSB.ToString());
    }

    private void lcbcYAxisType_ListSelectionChanged(object? sender, Models.Events.ListSelectionChangedEventArgs e)
    {
        PopulateGridCharts();
    }

    private void lcbcXAxisType_ListSelectionChanged(object? sender, Models.Events.ListSelectionChangedEventArgs e)
    {
        if (e.SelectedItem is not ListableItem item) return;
        if (item.Tag is not mctcPlotAxisType axesType) return;
        lcbcXAxisVariable.ViewModel.IsReadOnly = axesType != mctcPlotAxisType.Observed;
        PopulateGridCharts();
    }

    private void lcbcXAxisVariable_ListSelectionChanged(object? sender, Models.Events.ListSelectionChangedEventArgs e)
    {
        PopulateGridCharts();
    }
}