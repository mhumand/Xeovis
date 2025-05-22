using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
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

public partial class mctcPlotSingleViewTabControl : UserControl, ImctcPlotView
{
    public Guid Id { get; set; }
    public mctcPlotViewType ViewType { get; set; }

    private mctcPlotSingleViewModel _viewModel = new mctcPlotSingleViewModel();
    public mctcPlotSingleViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new mctcPlotSingleViewModel();
            DataContext = _viewModel;
            PrepareUI();
        }
    }

    public MainWindow? ParentWindow { get; set; }

    public mctcPlotSingleViewTabControl()
    {
        InitializeComponent();
        if (this.Content is Control content)
            content.DataContext = this.DataContext;
    }

    private void PrepareUI()
    {
        var vmChart = new ChartControlViewModel();
        chartControl.ViewModel = vmChart;
        chartControl.EnableCrosshairPointSelection();
        chartControl.ChartPointSelected += chartControl_ChartPointSelected;

        var vmTable = new TableControlViewModel();
        tableControl.ViewModel = vmTable;
        tableControl.SelectionChanged += tableControl_SelectionChanged;

        var vmXeosVariable = new LabelComboBoxControlViewModel();
        vmXeosVariable.Label = "x-eos Variable";
        lcbcXeosVariable.ViewModel = vmXeosVariable;
        lcbcXeosVariable.ListSelectionChanged += lcbcXeosVariable_ListSelectionChanged;

        var vmSpinner = new LabelSpinnerControlViewModel("mctc Run No.", 1, 1, 1);
        spinner.ViewModel = vmSpinner;
        spinner.ValueChanged += spinner_ValueChanged;

        tableFilter.CellValueChanged += tableFilter_CellValueChanged;
    }

    public bool PopulateUI(mctcPlotDto mctcPlotDto, mctcBundle mctcBundle)
    {
        if (mctcPlotDto == null || mctcBundle == null) return false;

        _viewModel.mctcPlotDto = mctcPlotDto;
        _viewModel.mctcBundle = mctcBundle;

        spinner.ViewModel.MaxValue = mctcBundle.mctcVariables[0].Values.Count;

        _viewModel.ClearInUseMarkersAndColors();
        PopulateMctcRunsTable();
        PopulatePhaseFilters();
        lcbcXeosVariable.SetItems(_viewModel.GetXeosVarItems());

        return true;
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

    private void lcbcXeosVariable_ListSelectionChanged(object? sender, Models.Events.ListSelectionChangedEventArgs e)
    {
        UpdateChartAndTable();
    }

    private void UpdateChartAndTable()
    {
        if (lcbcXeosVariable.ViewModel.SelectedItem is not ListableItem item) return;
        if (item.Tag is not xeosVariable sampleVar) return;

        var successful = _viewModel.GroupAndFilterXeosVariables(sampleVar);
        if (!successful)
        {
            tableControl.ClearTable();
            chartControl.ClearPlot();
            stackLegend.Children.Clear();
            return;
        }

        PopulateChart();
        PopulateTable();
        PopulateLegend();
    }

    private void PopulateLegend()
    {
        stackLegend.Children.Clear();
        var checkboxes = new List<CheckBoxControl>();
        foreach (var curve in chartControl.ViewModel.ChartModel.Curves)
        {
            var markerImage = chartControl.GetMarkerGraphic(curve.Id);
            if (markerImage == null || !curve.IsVisibleInLegend) continue;
            var checkboxViewModel = new CheckBoxControlViewModel(curve.Name, isChecked: curve.IsVisible) { Image = markerImage };
            var checkboxControl = new CheckBoxControl() { ViewModel = checkboxViewModel, Tag = curve };
            checkboxControl.CheckStateChanged += checkboxControl_CheckStateChanged;
            checkboxControl.PointerPressed += checkboxControl_PointerPressed;
            checkboxControl.ContextMenu = GetLegendCheckboxContextMenu();
            checkboxControl.Margin = new Avalonia.Thickness(5, 0);
            checkboxes.Add(checkboxControl);
        }

        if (_viewModel.mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Batch)
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
        if (checkbox.Tag is not CurveModel curve) return;
        chartControl.SetCurveVisibilty(curve.Id, e.IsChecked);
    }

    private void PopulateChart()
    {
        var chartModel = _viewModel.CreateChartModel();
        var successful = chartControl.PopulateChart(chartModel);
    }

    private void PopulateTable()
    {
        var tableModel = _viewModel.CreateTableModel();
        var successful = tableControl.PopulateTable(tableModel);
    }

    private void chartControl_ChartPointSelected(object? sender, ChartPointSelectedEventArgs e)
    {
        int index = -1;
        if (e != null && e.Index >= 0 && e.CurveId != Guid.Empty)
        {
            var columns = tableControl.ViewModel.TableModel.Columns;
            var curve = chartControl.ViewModel.ChartModel.Curves.FirstOrDefault(c => c.Id == e.CurveId);
            if (curve != null)
            {
                if (_viewModel.mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Batch)
                {
                    index = Enumerable.Range(0, columns.Min(c => c.DataCount)).ToList().FirstOrDefault(i => (double)columns[1].Values[i] == e.X && (double)columns[2].Values[i] == e.Y && columns[3].Values[i].ToString() == curve.Name, -1);
                }
                else if (_viewModel.mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Temperature)
                {
                    index = Enumerable.Range(0, columns.Min(c => c.DataCount)).ToList().FirstOrDefault(i => (double)columns[1].Values[i] == e.X && (double)columns[2].Values[i] == e.Y && columns[6].Values[i].ToString() == curve.Name, -1);
                }
                else if (_viewModel.mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Pressure)
                {
                    index = Enumerable.Range(0, columns.Min(c => c.DataCount)).ToList().FirstOrDefault(i => (double)columns[1].Values[i] == e.X && (double)columns[2].Values[i] == e.Y && columns[5].Values[i].ToString() == curve.Name, -1);
                }
            }
        }

        tableControl.SelectRow(index);
    }

    private void tableControl_SelectionChanged(object? sender, TableSelectionChangedEventArgs e)
    {
        if (e.Value == null)
        {
            chartControl.SelectPoint(Guid.Empty, -1);
            return;
        }

        var values = ((Dictionary<string, object>)e.Value).Values.ToList();
        var x = DoubleExtensions.GetDoubleValueFromText(values[1].ToString());
        var y = DoubleExtensions.GetDoubleValueFromText(values[2].ToString());

        var name = _viewModel.mctcPlotDto.PointsGroupingType switch
        {
            mctcPlotPointsGroupingType.Batch => values[3].ToString(),
            mctcPlotPointsGroupingType.Temperature => values[6].ToString(),
            mctcPlotPointsGroupingType.Pressure => values[5].ToString(),
            _ => null
        };

        var curve = chartControl.ViewModel.ChartModel.Curves.FirstOrDefault(c => c.Name == name);
        if (curve == null) return;
        var index = Enumerable.Range(0, Math.Min(curve.Xvals.Count, curve.Yvals.Count)).FirstOrDefault(i => curve.Xvals[i] == x && curve.Yvals[i] == y, -1);
        if (index < 0)
            chartControl.SelectPoint(Guid.Empty, -1);
        else
            chartControl.SelectPoint(curve.Id, index);
    }

    private void spinner_ValueChanged(object? sender, Models.Events.SpinnerValueChangedEventArgs e)
    {
        _viewModel.mctcRunIndex = e.Value - 1;
        PopulateMctcRunsTable();
        UpdateChartAndTable();
    }

    private void tableFilter_CellValueChanged(object? sender, TableCellValueChangedEventArgs e)
    {
        var column = tableFilter.ViewModel.TableModel.GetColumnById(e.ColumnId);
        if (column != null && column.HasOptions && !column.IsReadOnly)
        {
            var key = _viewModel.PhaseFilters.Keys.ToList()[e.RowIndex];
            _viewModel.PhaseFilters[key] = EnumExtensions.GetValueFromDescription<FilterType>(e.Value.ToString());
            UpdateChartAndTable();
        }
    }

    private void btnClearFilters_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        PopulatePhaseFilters();
        UpdateChartAndTable();
    }

    private void btnSidePanel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        splitView.IsPaneOpen = !splitView.IsPaneOpen;
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