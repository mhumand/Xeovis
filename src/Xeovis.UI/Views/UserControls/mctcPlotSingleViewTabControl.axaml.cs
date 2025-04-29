using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Base;
using Xeovis.Domain;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.ViewModels;

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

    public mctcPlotSingleViewTabControl()
    {
        InitializeComponent();
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
        lcbcXeosVariable.SetItems(_viewModel.GetXeosVarItems());

        PopulateMctcRunsTable();
        PopulatePhaseFilters();

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
            return;
        }

        PopulateChart();
        PopulateTable();
        PopulateLegend();
    }

    private void PopulateLegend()
    {
        stackLegend.Children.Clear();
        foreach (var curve in chartControl.ViewModel.ChartModel.Curves)
        {
            var markerImage = chartControl.GetMarkerGraphic(curve.Id);
            if (markerImage == null || !curve.IsVisibleInLegend) continue;
            var checkboxViewModel = new CheckBoxControlViewModel(curve.Name, isChecked: curve.IsVisible) { Image = markerImage };
            var checkboxControl = new CheckBoxControl() { ViewModel = checkboxViewModel, Tag = curve };
            checkboxControl.CheckStateChanged += checkboxControl_CheckStateChanged;
            checkboxControl.PointerPressed += checkboxControl_PointerPressed;
            checkboxControl.ContextMenu = GetLegendCheckboxContextMenu();
            stackLegend.Children.Add(checkboxControl);
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

    private void chartControl_ChartPointSelected(object? sender, Models.Events.ChartPointSelectedEventArgs e)
    {
        int index = -1;
        if (e.Index >= 0 && e.CurveId != Guid.Empty)
        {
            var columns = tableControl.ViewModel.TableModel.Columns;
            var curve = chartControl.ViewModel.ChartModel.Curves.FirstOrDefault(c => c.Id == e.CurveId);
            if (curve != null)
            {
                index = Enumerable.Range(0, columns.Min(c => c.DataCount)).ToList().FirstOrDefault(i => (double)columns[1].Values[i] == e.X && (double)columns[2].Values[i] == e.Y && columns[3].Values[i].ToString() == curve.Name, -1);
            }
        }

        tableControl.SelectRow(index);
    }

    private void tableControl_SelectionChanged(object? sender, Models.Events.TableSelectionChangedEventArgs e)
    {
        if (e.Value == null)
        {
            chartControl.SelectPoint(Guid.Empty, -1);
            return;
        }

        var values = ((Dictionary<string, object>)e.Value).Values.ToList();
        var x = DoubleExtensions.GetDoubleValueFromText(values[1].ToString());
        var y = DoubleExtensions.GetDoubleValueFromText(values[2].ToString());
        var name = values[3].ToString();
        var curve = chartControl.ViewModel.ChartModel.Curves.FirstOrDefault(c => c.Name == name);
        if (curve == null) return;
        var indexX = curve.Xvals.IndexOf(x);
        var indexY = curve.Yvals.IndexOf(y);
        if (indexX < 0 || indexY < 0 || indexX != indexY) return;
        chartControl.SelectPoint(curve.Id, indexY);
    }

    private void spinner_ValueChanged(object? sender, Models.Events.SpinnerValueChangedEventArgs e)
    {
        _viewModel.mctcRunIndex = e.Value - 1;
        PopulateMctcRunsTable();
        UpdateChartAndTable();
    }

    private void tableFilter_CellValueChanged(object? sender, Models.Events.TableCellValueChangedEventArgs e)
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
}