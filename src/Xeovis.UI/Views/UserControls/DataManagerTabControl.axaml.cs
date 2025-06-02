using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Domain;
using Xeovis.UI.Models;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class DataManagerTabControl : UserControl
{
    private DataManagerTabControlViewModel _viewModel = new DataManagerTabControlViewModel();

    public DataManagerTabControlViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value;
        }
    }

    public DataManagerTabControl()
    {
        InitializeComponent();
        PrepareUI();
    }

    private void PrepareUI()
    {
        lcbExperiment.ViewModel = new LabelComboBoxControlViewModel() { Label = "Select experiment", IsStackedHorizontally = true };
        lcbExperiment.ListSelectionChanged += lcbExperiment_ListSelectionChanged;

        lcbPhaseInExperiment.ViewModel = new LabelComboBoxControlViewModel() { Label = "Select phase", IsStackedHorizontally = true };
        lcbPhaseInExperiment.ListSelectionChanged += lcbPhaseInExperiment_ListSelectionChanged;

        lcbExpRunInExperiment.ViewModel = new LabelComboBoxControlViewModel() { Label = "Select run", IsStackedHorizontally = true };
        lcbExpRunInExperiment.ListSelectionChanged += lcbExpRunInExperiment_ListSelectionChanged;

        lcbPhases.ViewModel = new LabelComboBoxControlViewModel() { Label = "Select phase", IsStackedHorizontally = true };
        lcbPhases.ListSelectionChanged += lcbPhases_ListSelectionChanged;
    }

    public void SetExperiments(List<Experiment> experiments)
    {
        tableExperiment.ClearTable();
        tablePhaseInExperiment.ClearTable();
        tableExpRunInExperiment.ClearTable();
        tablePhases.ClearTable();
        tablePhasesSummary.ClearTable();

        lcbExperiment.ClearItems();
        lcbPhaseInExperiment.ClearItems();
        lcbExpRunInExperiment.ClearItems();
        lcbPhases.ClearItems();

        _viewModel.CreateExperimentItems(experiments);
        lcbExperiment.SetItems(_viewModel.ExperimentItems.ToList());
        lcbPhases.SetItems(_viewModel.GetPhaseTypeItemList());
    }

    private void lcbPhases_ListSelectionChanged(object? sender, Models.Events.ListSelectionChangedEventArgs e)
    {
        tablePhases.ClearTable();
        tablePhasesSummary.ClearTable();
        if (e.SelectedItem is not ListableItem phaseItem) return;
        var table = _viewModel.CreatePhasesTableModel(phaseItem);
        tablePhases.PopulateTable(table);
        var summaryTable = _viewModel.CreatePhasesSummaryTableModel();
        tablePhasesSummary.PopulateTable(summaryTable);
    }

    private void lcbPhaseInExperiment_ListSelectionChanged(object? sender, Models.Events.ListSelectionChangedEventArgs e)
    {
        tablePhaseInExperiment.ClearTable();
        if (e.SelectedItem is not ListableItem phaseInExperimentItem) return;
        var table = _viewModel.CreatePhaseInExperimentTableModel(lcbExperiment.ViewModel.SelectedItem, phaseInExperimentItem);
        tablePhaseInExperiment.PopulateTable(table);
    }

    private void lcbExpRunInExperiment_ListSelectionChanged(object? sender, Models.Events.ListSelectionChangedEventArgs e)
    {
        tableExpRunInExperiment.ClearTable();
        if (e.SelectedItem is not ListableItem expRunInExperimentItem) return;
        var table = _viewModel.CreateExpRunInExperimentTableModel(lcbExperiment.ViewModel.SelectedItem, expRunInExperimentItem);
        tableExpRunInExperiment.PopulateTable(table);
    }

    private void lcbExperiment_ListSelectionChanged(object? sender, Models.Events.ListSelectionChangedEventArgs e)
    {
        tableExperiment.ClearTable();
        lcbPhaseInExperiment.ClearItems();
        lcbExpRunInExperiment.ClearItems();
        if (e.SelectedItem is not ListableItem experimentItem) return;
        var table = _viewModel.CreateExperimentTableModel(experimentItem);
        tableExperiment.PopulateTable(table);

        _viewModel.CreatePhaseInExperimentItems(experimentItem);
        lcbPhaseInExperiment.SetItems(_viewModel.PhaseInExperimentItems.OrderBy(i => i.Label).ToList());

        _viewModel.CreateExpRunInExperimentItems(experimentItem);
        lcbExpRunInExperiment.SetItems(_viewModel.ExpRunInExperimentItems.ToList());
    }

    public void DeleteExperiments(List<Guid> Ids)
    {
        _viewModel.DeleteExperiments(Ids);
        lcbExperiment.SetItems(_viewModel.ExperimentItems.ToList());
        lcbPhases.SetItems(_viewModel.GetPhaseTypeItemList());
    }

    public void OpenExperiment(Guid Id)
    {
        var item = _viewModel.ExperimentItems.FirstOrDefault(x => x.PresenterId == Id);
        lcbExperiment.ViewModel.SelectedItem = item;
        tabControl.SelectedItem = tabExperiments;
    }

    public void OpenPhaseInExperiment(Guid experimentId, PhaseType phaseType)
    {
        OpenExperiment(experimentId);
        lcbPhaseInExperiment.ViewModel.SelectedItem = _viewModel.PhaseInExperimentItems.FirstOrDefault(x => (PhaseType)x.Tag == phaseType);
    }

    public void OpenExpRunInExperiment(Guid experimentId, Guid expRunId)
    {

    }
}