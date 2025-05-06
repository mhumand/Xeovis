using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Base;
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

    public mctcPlotGridViewTabControl()
    {
        InitializeComponent();
    }

    private void PrepareUI()
    {
        var vmSpinnerMctcRun = new LabelSpinnerControlViewModel("mctc Run No.", 1, 1, 1);
        spinnerMctcRun.ViewModel = vmSpinnerMctcRun;
        spinnerMctcRun.ValueChanged += spinnerMctcRun_ValueChanged;

        var vmSpinnerColumnNo = new LabelSpinnerControlViewModel("Column No.", 1, 1, 1);
        spinnerColumnNo.ViewModel = vmSpinnerColumnNo;
        spinnerColumnNo.ValueChanged += spinnerColumnNo_ValueChanged;

        slider.ValueChanged += slider_ValueChanged;
        slider.Minimum = uniformGrid.Bounds.Width;
        slider.Maximum = 5000;
        slider.Value = uniformGrid.Bounds.Width;
        tableFilter.CellValueChanged += tableFilter_CellValueChanged;
    }

    public bool PopulateUI(mctcPlotDto mctcPlotDto, mctcBundle mctcBundle)
    {
        if (mctcPlotDto == null || mctcBundle == null) return false;

        _viewModel.mctcPlotDto = mctcPlotDto;
        _viewModel.mctcBundle = mctcBundle;

        spinnerMctcRun.ViewModel.MaxValue = mctcBundle.mctcVariables[0].Values.Count;
        spinnerColumnNo.ViewModel.MaxValue = mctcPlotDto.SampleXeosVariables.Count;
        spinnerColumnNo.ViewModel.Value = Math.Min(3, mctcPlotDto.SampleXeosVariables.Count);

        _viewModel.ClearInUseMarkersAndColors();
        PopulateMctcRunsTable();
        PopulatePhaseFilters();
        PopulateGridCharts();

        return true;
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
        var chartControls = new List<ChartControl>();
        foreach (var sampleVar in _viewModel.mctcPlotDto.SampleXeosVariables)
        {
            var viewModel = new ChartControlViewModel();
            var chartControl = new ChartControl();
            chartControl.ViewModel = viewModel;
            chartControl.SetScrollZooming(false);

            var chartModel = _viewModel.CreateChartModel(sampleVar);
            var successful = chartControl.PopulateChart(chartModel);
            if (successful)
                chartControls.Add(chartControl);
        }

        uniformGrid.Children.Clear();
        uniformGrid.Columns = (int)spinnerColumnNo.ViewModel.Value;
        uniformGrid.Rows = chartControls.Count / uniformGrid.Columns + 1;
        foreach (var chartControl in chartControls)
            uniformGrid.Children.Add(chartControl);

        PopulateLegend();
    }

    private void PopulateLegend()
    {
        stackLegend.Children.Clear();
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
                stackLegend.Children.Add(checkboxControl);
            }
        }
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
        uniformGrid.Columns = e.Value;
        uniformGrid.Rows = uniformGrid.Children.Count / e.Value + 1;
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
}