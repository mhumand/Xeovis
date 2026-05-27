using Avalonia.Controls;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Input;
using Avalonia.Interactivity;
using Xeovis.Base;
using Xeovis.Base.Comparers;
using Xeovis.Base.Utilities;
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

    private bool _isDraggingLegend = false;
    private bool _dragTargetState = false;
    private CheckBoxControl? _lastHoveredCheckbox = null;
    
    public MainWindow? ParentWindow { get; set; }

    private bool _linkVariable;

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
        lcbcYAxisVariable.ViewModel = vmYAxisVariable;
        lcbcYAxisVariable.ListSelectionChanged += lcbcYAxisVariable_ListSelectionChanged;
        
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

        var vmSpinner = new LabelSpinnerControlViewModel("mctc Run No.", 1, 1, 1);
        spinner.ViewModel = vmSpinner;
        spinner.ValueChanged += spinner_ValueChanged;

        tableFilter.CellValueChanged += tableFilter_CellValueChanged;
        tableControl.ContextRequested += tableControl_ContextRequested;
        toggleLink.IsCheckedChanged += toggleLink_IsCheckedChanged;
        _linkVariable = (bool)toggleLink.IsChecked;
        
        stackLegend.AddHandler(PointerPressedEvent, StackLegend_PointerPressed, RoutingStrategies.Tunnel);
        stackLegend.AddHandler(PointerMovedEvent, StackLegend_PointerMoved, RoutingStrategies.Tunnel);
        stackLegend.AddHandler(PointerReleasedEvent, StackLegend_PointerReleased, RoutingStrategies.Tunnel);
    }
    
    
    private void StackLegend_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(stackLegend);
        
        // We only want to handle left-click drags. 
        // Your existing Middle-Click "CheckThisOnly" logic will safely ignore this.
        if (!point.Properties.IsLeftButtonPressed) return;

        var pos = e.GetPosition(stackLegend);
        
        foreach (Control child in stackLegend.Children)
        {
            // Find the exact checkbox the user clicked down on
            if (child is CheckBoxControl chk && chk.Bounds.Contains(pos))
            {
                _isDraggingLegend = true;
                
                // If it's unchecked, we drag to CHECK. If checked, we drag to UNCHECK.
                _dragTargetState = !chk.ViewModel.IsChecked;
                
                // Toggle the first item immediately
                chk.ViewModel.IsChecked = _dragTargetState;
                _lastHoveredCheckbox = chk;
                
                // Capture the mouse so we don't lose tracking if the user drags quickly
                e.Pointer.Capture(stackLegend);
                
                // Mark as handled so the inner CheckBox doesn't ALSO process the click 
                // and accidentally double-toggle itself.
                e.Handled = true; 
                break;
            }
        }
    }

    private void StackLegend_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_isDraggingLegend) return;

        var pos = e.GetPosition(stackLegend);
        
        foreach (Control child in stackLegend.Children)
        {
            // Find which checkbox the mouse is currently hovering over
            if (child is CheckBoxControl chk && chk.Bounds.Contains(pos))
            {
                // Only update the ViewModel if we moved into a NEW checkbox
                if (chk != _lastHoveredCheckbox)
                {
                    chk.ViewModel.IsChecked = _dragTargetState;
                    _lastHoveredCheckbox = chk;
                }
                break;
            }
        }
    }

    private void StackLegend_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        // End the drag only if we own the capture
        if (_isDraggingLegend && e.Pointer.Captured == stackLegend)
        {
            _isDraggingLegend = false;
            _lastHoveredCheckbox = null;
            
            // Release the mouse capture back to the OS
            e.Pointer.Capture(null); 
            e.Handled = true;
        }
    }
    

    public bool PopulateUI(mctcPlotDto mctcPlotDto, mctcBundle mctcBundle)
    {
        if (mctcPlotDto == null || mctcBundle == null) return false;

        _viewModel.mctcPlotDto = mctcPlotDto;
        _viewModel.mctcBundle = mctcBundle;

        spinner.ViewModel.MaxValue = mctcBundle.mctcVariables[0].Values.Count;
        spinner.ValueChanged -= spinner_ValueChanged;
        spinner.ViewModel.Value = 1;
        _viewModel.mctcRunIndex = 0;
        spinner.ValueChanged += spinner_ValueChanged;

        _viewModel.ClearInUseMarkersAndColors();
        PopulateMctcRunsTable();
        PopulatePhaseFilters();
        lcbcYAxisVariable.SetItems(_viewModel.GetXeosVarItems());
        lcbcXAxisVariable.SetItems(_viewModel.GetXeosVarItems());

        return true;
    }

    public void Refresh()
    {
        PopulateUI(_viewModel.mctcPlotDto, _viewModel.mctcBundle);
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

    private void lcbcYAxisType_ListSelectionChanged(object? sender, Models.Events.ListSelectionChangedEventArgs e)
    {
        UpdateChartAndTable();
    }
    
    private void lcbcYAxisVariable_ListSelectionChanged(object? sender, Models.Events.ListSelectionChangedEventArgs e)
    {
        lcbcYAxisVariable.ListSelectionChanged -= lcbcYAxisVariable_ListSelectionChanged;
        LinkVariableComboboxes(lcbcYAxisVariable, lcbcXAxisVariable);
        lcbcYAxisVariable.ListSelectionChanged += lcbcYAxisVariable_ListSelectionChanged;

        UpdateChartAndTable();
    }

    private void lcbcXAxisType_ListSelectionChanged(object? sender, Models.Events.ListSelectionChangedEventArgs e)
    {
        if (e.SelectedItem is not ListableItem item) return;
        if (item.Tag is not mctcPlotAxisType axesType) return;
        lcbcXAxisVariable.ViewModel.IsReadOnly = axesType != mctcPlotAxisType.Observed;
        UpdateChartAndTable();
    }
    
    private void lcbcXAxisVariable_ListSelectionChanged(object? sender, Models.Events.ListSelectionChangedEventArgs e)
    {
        lcbcXAxisVariable.ListSelectionChanged -= lcbcXAxisVariable_ListSelectionChanged;
        LinkVariableComboboxes(lcbcXAxisVariable, lcbcYAxisVariable);
        lcbcXAxisVariable.ListSelectionChanged += lcbcXAxisVariable_ListSelectionChanged;

        UpdateChartAndTable();
    }
    
    private void UpdateChartAndTable()
    {
        if (lcbcYAxisVariable.ViewModel.SelectedItem is not ListableItem yVarItem) return;
        if (lcbcXAxisVariable.ViewModel.SelectedItem is not ListableItem xVarItem) return;
        if (yVarItem.Tag is not xeosVariable YAxisVariable) return;
        if (xVarItem.Tag is not xeosVariable XAxisVariable) return;

        if (lcbcYAxisType.ViewModel.SelectedItem is not ListableItem yAxisType) return;
        if (lcbcXAxisType.ViewModel.SelectedItem is not ListableItem xAxisType) return;
        if (yAxisType.Tag is not mctcPlotAxisType YAxisType) return;
        if (xAxisType.Tag is not mctcPlotAxisType XAxisType) return;

        var axesSettings = new mctcPlotAxesSettings(YAxisVariable, XAxisVariable, YAxisType, XAxisType);
        var successful = _viewModel.GroupAndFilterXeosVariables(axesSettings);
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
                var range = Enumerable.Range(0, columns.Min(c => c.DataCount)).ToList();
                if (_viewModel.mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Batch)
                {
                    index = range.FirstOrDefault(i => (double)columns[1].Values[i] == e.X && (double)columns[2].Values[i] == e.Y 
                    && columns[3].Values[i].ToString() == curve.Name, -1);
                }
                else if (_viewModel.mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Experiment)
                {
                    index = range.FirstOrDefault(i => (double)columns[1].Values[i] == e.X && (double)columns[2].Values[i] == e.Y 
                    && columns[4].Values[i].ToString().Split(" - ", StringSplitOptions.RemoveEmptyEntries)[1] == curve.Name, -1);
                }
                else if (_viewModel.mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.ExperimentalRun)
                {
                    index = range.FirstOrDefault(i => (double)columns[1].Values[i] == e.X && (double)columns[2].Values[i] == e.Y 
                        && string.Join(" - ", columns[4].Values[i].ToString().Split(" - ").Skip(1)) == curve.Name, -1);
                }
                else if (_viewModel.mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Temperature)
                {
                    index = range.FirstOrDefault(i => (double)columns[1].Values[i] == e.X && (double)columns[2].Values[i] == e.Y 
                    && columns[6].Values[i].ToString() == curve.Name, -1);
                }
                else if (_viewModel.mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Pressure)
                {
                    index = range.FirstOrDefault(i => (double)columns[1].Values[i] == e.X && (double)columns[2].Values[i] == e.Y 
                    && columns[5].Values[i].ToString() == curve.Name, -1);
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
            mctcPlotPointsGroupingType.Experiment => values[4].ToString().Split(" - ", StringSplitOptions.RemoveEmptyEntries)[1].ToString(),
            mctcPlotPointsGroupingType.ExperimentalRun => string.Join(" - ", values[4].ToString().Split(" - ").Skip(1)) ,
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

    private void tableControl_ContextRequested(object? sender, ContextRequestedEventArgs e)
    {
        if (ParentWindow == null) return;
        var experiments = ParentWindow.ViewModel.Experiments;
        if (experiments == null || experiments.Count == 0) return;

        // reading data
        var row = tableControl.dataGrid.SelectedItem;
        if (row == null) return;
        var values = ((Dictionary<string, object>)row).Values.Select(x => x.ToString()).ToList();
        if (values.Count == 0) return;
        var phaseNames = values[3].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var runData = values[4].Split(['-', ' '], StringSplitOptions.RemoveEmptyEntries);

        var runP = DoubleExtensions.GetDoubleValueFromText(values[5]);
        var runT = DoubleExtensions.GetDoubleValueFromText(values[6]);

        var expName = runData[1];
        var expRunName = runData[2];
        var batchIndex = runData[0];

        // checking data
        var experiment = experiments.FirstOrDefault(x => x.ShortName == expName);
        if (experiment == null) return;
        var runNameSimilarity = new Dictionary<ExpRun, int>();
        experiment.Runs.ForEach(x => runNameSimilarity.Add(x, StringUtilities.GetLevenshteinDistance(x.Name, expRunName)));
        runNameSimilarity = runNameSimilarity.OrderBy(x => x.Value).ToDictionary();
        var expRun = runNameSimilarity.Keys.FirstOrDefault();
        if (runNameSimilarity[expRun] > 5)
        {
            Log.Information("ExpRun was found but name is too dissimilar [{n1},{n2}]. Searching based on T and P ...", expRun.Name, expRunName);
            expRun = experiment.Runs.FirstOrDefault(x => x.Temperature == runT && x.Pressure == runP);
        }

        // constructing menu
        var menu = new ContextMenu();

        var gotoExpItem = new MenuItem() { Header = $"Go to Experiment" };
        gotoExpItem.Click += (s, e) => ParentWindow.OpenExperiment(experiment.Id);
        ToolTip.SetTip(gotoExpItem, experiment.Name);
        menu.Items.Add(gotoExpItem);

        if (expRun != null)
        {
            var gotoExpRunItem = new MenuItem() { Header = $"Go to Run" };
            gotoExpRunItem.Click += (s, e) => ParentWindow.OpenExpRunInExperiment(experiment.Id, expRun.Id);
            ToolTip.SetTip(gotoExpRunItem, expRun.Name);
            menu.Items.Add(gotoExpRunItem);
        }

        if (expRun != null)
        {
            var gotoPhaseItem = new MenuItem() { Header = "Go to Phase" };
            var addToCampareItem = new MenuItem() { Header = "Add to Compare" };

            foreach (var phaseName in phaseNames)
            {
                var phaseInExpRun = expRun.Phases.FirstOrDefault(x => x.NameSet != null && x.NameSet.HasName(phaseName));
                if (phaseInExpRun == null || phaseInExpRun.Composition.Count == 0) continue;
                var phaseDataItem = new MenuItem() { Header = phaseName };
                phaseDataItem.Click += (s, e) => ParentWindow.OpenPhaseInExperiment(experiment.Id, phaseInExpRun.NameSet, expRun.Id);
                gotoPhaseItem.Items.Add(phaseDataItem);

                var phaseCompareItem = new MenuItem() { Header = phaseName };
                phaseCompareItem.Click += (s, e) => ParentWindow.AddPhaseToCompare(phaseInExpRun.Id);
                addToCampareItem.Items.Add(phaseCompareItem);
            }

            if (gotoPhaseItem.Items.Count > 0)
            {
                menu.Items.Add(gotoPhaseItem);
                menu.Items.Add(new Separator());
                menu.Items.Add(addToCampareItem);
            }
        }

        menu.Open(tableControl);
    }

    private void toggleLink_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _linkVariable = (bool)toggleLink.IsChecked;
        LinkVariableComboboxes(lcbcYAxisVariable, lcbcXAxisVariable);
    }

    private void LinkVariableComboboxes(LabelComboBoxControl source, LabelComboBoxControl destination)
    {
        if (!_linkVariable) return;
        if (source.ViewModel.SelectedItem is not ListableItem item) return;
        if (item.Tag is not xeosVariable sampleVar) return;
        var itemInDestination = destination.ViewModel.Items.FirstOrDefault(x => x.Tag is xeosVariable var && var.IsEqualTo(sampleVar));
        if (itemInDestination == null) return;
        destination.ViewModel.SelectedItem = itemInDestination;
    }
}