using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xeovis.Domain;
using Xeovis.Persistence;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class mctcPlotSingleViewTabControl : UserControl, ImctcPlotView
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public mctcPlotViewType ViewType { get; set; }

    private mctcPlotSingleViewModel _viewModel;
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

        var vmXeosVariable = new LabelComboBoxControlViewModel();
        vmXeosVariable.Label = "x-eos Variable";
        lcbcXeosVariable.ViewModel = vmXeosVariable;
        lcbcXeosVariable.ListSelectionChanged += lcbcXeosVariable_ListSelectionChanged;

        var vmSpinner = new LabelSpinnerControlViewModel("mctc Run No.", 1, 1, 1);
        spinner.ViewModel = vmSpinner;
        spinner.ValueChanged += spinner_ValueChanged;
    }

    public bool FillUI(mctcPlotDto mctcPlotDto, mctcBundle mctcBundle)
    {
        if (mctcPlotDto == null || mctcBundle == null) return false;

        _viewModel.mctcPlotDto = mctcPlotDto;
        _viewModel.mctcBundle = mctcBundle;

        spinner.ViewModel.MaxValue = mctcBundle.mctcVariables[0].Values.Count;
        lcbcXeosVariable.SetItems(_viewModel.GetXeosVarItems());

        FillMctcRunsTable();

        return true;
    }

    private void FillMctcRunsTable()
    {
        var tableModel = _viewModel.CreateMctcRunsTableModel((int)spinner.ViewModel.Value - 1);
        tableMctcRuns.PopulateTable(tableModel);
    }

    private void lcbcXeosVariable_ListSelectionChanged(object? sender, Models.Events.ListSelectionChangedEventArgs e)
    {
        UpdateChartAndTable();
    }

    private void UpdateChartAndTable()
    {
        if (lcbcXeosVariable.ViewModel.SelectedItem is not ListableItem item) return;
        if (item.Tag is not xeosVariable sampleVar) return;

        var xeosVariables = _viewModel.mctcBundle.xeosVariables.Where(x => x.VariableType == sampleVar.VariableType && x.PhaseType == sampleVar.PhaseType).ToList();
        var xeosVarGroups = _viewModel.GroupXeosVariables(xeosVariables);
        if (xeosVarGroups.Count == 0) return;
        PopulateChart(xeosVarGroups);
        PopulateTable(xeosVarGroups);
    }

    private void PopulateChart(List<IGrouping<object, xeosVariable>> groups)
    {
        if (groups == null || groups.Count == 0) return;
        if (lcbcXeosVariable.ViewModel.SelectedItem.Tag is not xeosVariable sampleVar) return;

        var chartModel = _viewModel.CreateChartModel(groups, sampleVar, (int)spinner.ViewModel.Value - 1);
        var successful = chartControl.PopulateChart(chartModel);
    }

    private void PopulateTable(List<IGrouping<object, xeosVariable>> groups)
    {
        if (groups == null || groups.Count == 0) return;
        if (lcbcXeosVariable.ViewModel.SelectedItem.Tag is not xeosVariable sampleVar) return;

        var tableModel = _viewModel.CreateTableModel(groups, sampleVar, (int)spinner.ViewModel.Value - 1);
        var successful = tableControl.PopulateTable(tableModel);
    }

    private void chartControl_ChartPointSelected(object? sender, Models.Events.ChartPointSelectedEventArgs e)
    {
        // select row in table.
    }

    private void spinner_ValueChanged(object? sender, Models.Events.SpinnerValueChangedEventArgs e)
    {
        FillMctcRunsTable();
        UpdateChartAndTable();
    }
}