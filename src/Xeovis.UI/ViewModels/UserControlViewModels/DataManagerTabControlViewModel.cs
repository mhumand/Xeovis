using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xeovis.Base;
using Xeovis.Domain;
using Xeovis.Presentation;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels
{
    public partial class DataManagerTabControlViewModel : TabControlViewModelBase
    {
        [ObservableProperty] private ObservableCollection<ListableItem> experimentItems;

        [ObservableProperty] private ObservableCollection<ListableItem> phaseInExperimentItems;

        [ObservableProperty] private ObservableCollection<ListableItem> expRunInExperimentItems;

        private List<Experiment> _experiments;
        private List<Phase> _allPhaseInstances;

        public DataManagerTabControlViewModel()
        {
            ExperimentItems = new ObservableCollection<ListableItem>();
            PhaseInExperimentItems = new ObservableCollection<ListableItem>();
            ExpRunInExperimentItems = new ObservableCollection<ListableItem>();

            CanBeRemoved = false;
            _experiments = new List<Experiment>();
            _allPhaseInstances = new List<Phase>();
        }

        public DataManagerTabControlViewModel(List<Experiment> experiments) : this()
        {
            CreateExperimentItems(experiments);
        }

        public void CreateExperimentItems(List<Experiment> experiments)
        {
            _allPhaseInstances.Clear();
            _experiments = experiments;

            ExperimentItems.Clear();
            foreach (var experiment in _experiments)
            {
                ExperimentItems.Add(new ListableItem(experiment.Name, description: experiment.ShortName)
                { PresenterId = experiment.Id });
            }
        }

        public TableModel CreateExperimentTableModel(ListableItem experimentItem)
        {
            var exp = _experiments.FirstOrDefault(e => e.Id == experimentItem.PresenterId);
            if (exp == null || exp.Runs.Count == 0) return null;

            var columns = new List<ColumnModel>
            {
                new ColumnModel("#", exp.Runs.Select(r => r.Index).ToList()),
                new ColumnModel("Name", exp.Runs.Select(r => r.Name).ToList()),
                new ColumnModel("P [Kbar]", exp.Runs.Select(r => r.Pressure).ToList()),
                new ColumnModel("T [°C]", exp.Runs.Select(r => r.Temperature).ToList()),
                new ColumnModel("t [hr]", exp.Runs.Select(r => r.Duration).ToList()),
                new ColumnModel("Rocktype", exp.Runs.Select(r => r.Rocktype.ToString()).ToList()),
                new ColumnModel("Phases", exp.Runs.Select(r => r.GetPhaseAssemblageString()).ToList())
            };

            var allOxides = exp.Runs.SelectMany(x => x.BulkCompOxides).Distinct();
            foreach (var oxide in allOxides)
            {
                var values = exp.Runs.Select(r =>
                    r.BulkComposition.TryGetValue(oxide, out double value) ? value.Round(6) : 0).ToList();
                var columnModel = new ColumnModel(oxide.ToString(), values);
                columns.Add(columnModel);
            }
            columns.Add(new ColumnModel("Total", exp.Runs.Select(r => r.BulkCompValues.Sum().Round(6)).ToList()));

            return new TableModel(exp.Name, columns) { IsReadOnly = true, FrozenColumnCount = 2 };
        }

        public void CreatePhaseInExperimentItems(ListableItem experimentItem)
        {
            var exp = _experiments.FirstOrDefault(e => e.Id == experimentItem.PresenterId);
            if (exp == null) return;

            PhaseInExperimentItems.Clear();
            foreach (var phase in exp.PhasesWithData)
                PhaseInExperimentItems.Add(new ListableItem(EnumExtensions.GetDescriptionFromValue(phase),
                    description: phase.ToString(), tag: phase));
        }

        public TableModel CreatePhaseInExperimentTableModel(ListableItem experimentItem, ListableItem phaseItem)
        {
            var exp = _experiments.FirstOrDefault(e => e.Id == experimentItem.PresenterId);
            if (exp == null || exp.Runs.Count == 0) return null;

            var phaseType = PhaseTypeExtensions.GetPhaseTypeByName(phaseItem.Label);

            var allPhasesInExp = exp.Runs.Where(r => r.Phases.Any(ph => ph.Type == phaseType))
                .Select(run => run.Phases.FirstOrDefault(ph => ph.Type == phaseType)).ToList();

            // phase present in ExpRun product assemblage, but no composition provided.
            allPhasesInExp.RemoveAll(i => i.Composition.Count == 0);

            if (allPhasesInExp.Count == 0) return null;
            var allOxides = allPhasesInExp.SelectMany(x => x.CompositionOxides).Distinct();

            var columns = new List<ColumnModel>
            {
                new ColumnModel("Run",
                    allPhasesInExp.Select(i => exp.Runs.FirstOrDefault(r => r.Id == i.ExpRunId).ToString()).ToList()),
            };

            foreach (var oxide in allOxides)
            {
                var values = allPhasesInExp.Select(i =>
                    i.Composition.TryGetValue(oxide, out double value) ? value.Round(6) : 0).ToList();
                var columnModel = new ColumnModel(oxide.ToString(), values);
                columns.Add(columnModel);
            }
            columns.Add(new ColumnModel("Total", allPhasesInExp.Select(i => i.CompositionValues.Sum().Round(6)).ToList()));
            columns.Add(new ColumnModel(header: "Comment", allPhasesInExp.Select(i => i.Comment).ToList()));

            return new TableModel(phaseItem.Label, columns) { IsReadOnly = true, FrozenColumnCount = 1 };
        }

        public void CreateExpRunInExperimentItems(ListableItem experimentItem)
        {
            var exp = _experiments.FirstOrDefault(e => e.Id == experimentItem.PresenterId);
            if (exp == null) return;

            ExpRunInExperimentItems.Clear();
            foreach (var run in exp.Runs.Where(r => r.Phases.Any(p => p.CompositionValues.Count > 0)))
                ExpRunInExperimentItems.Add(new ListableItem($"{run.Index} - {run.Name}", description: $"{run.Pressure} kbar, {run.Temperature} °C")
                { PresenterId = run.Id });
        }

        public TableModel CreateExpRunInExperimentTableModel(ListableItem experimentItem, ListableItem expRunItem)
        {
            var exp = _experiments.FirstOrDefault(e => e.Id == experimentItem.PresenterId);
            if (exp == null || exp.Runs.Count == 0) return null;
            var expRun = exp.Runs.FirstOrDefault(r => r.Id == expRunItem.PresenterId);
            if (expRun == null) return null;

            var phases = expRun.Phases.Where(x => x.CompositionValues.Count > 0);
            var columns = new List<ColumnModel>
            {
                new ColumnModel("Phase", phases.Select(p => EnumExtensions.GetDescriptionFromValue(p.Type)).ToList())
            };

            var allOxides = phases.SelectMany(p => p.CompositionOxides).Distinct();
            foreach (var oxide in allOxides)
            {
                var values = phases.Select(i => i.Composition.TryGetValue(oxide, out double value) ? value.Round(6) : 0).ToList();
                var columnModel = new ColumnModel(oxide.ToString(), values);
                columns.Add(columnModel);
            }
            columns.Add(new ColumnModel("Total", phases.Select(i => i.CompositionValues.Sum().Round(6)).ToList()));
            columns.Add(new ColumnModel(header: "Comment", phases.Select(i => i.Comment).ToList()));

            return new TableModel(expRunItem.Label, columns) { IsReadOnly = true, FrozenColumnCount = 1 };
        }

        public List<ListableItem> GetPhaseTypeItemList()
        {
            var phases = _experiments.SelectMany(e => e.PhasesWithData).Distinct().ToList();
            var list = new List<ListableItem>();
            foreach (var phase in phases)
                list.Add(new ListableItem(EnumExtensions.GetDescriptionFromValue(phase), tag: phase,
                    description: phase.ToString()));

            return list.OrderBy(l => l.Label).ToList();
        }

        public TableModel CreatePhasesTableModel(ListableItem phaseItem)
        {
            _allPhaseInstances.Clear();
            var phaseType = (PhaseType)phaseItem.Tag;

            var allExpsWithThePhase = _experiments.Where(e => e.PhasesWithData.Contains(phaseType));
            if (allExpsWithThePhase.Count() == 0) return null;

            var allRunsWithThePhase = allExpsWithThePhase.SelectMany(e =>
                e.Runs.Where(r => r.Phases.Any(ph => ph.Type == phaseType))).ToList();
            if (allRunsWithThePhase.Count() == 0) return null;

            _allPhaseInstances = allRunsWithThePhase.Select(r => r.Phases.FirstOrDefault(p => p.Type == phaseType))
                .ToList();

            // phase present in ExpRun product assemblage, but no composition provided.
            _allPhaseInstances.RemoveAll(i => i.Composition.Count == 0);
            if (_allPhaseInstances.Count == 0) return null;

            var columns = new List<ColumnModel>
            {
                new ColumnModel("#",
                    Enumerable.Range(1, _allPhaseInstances.Count()).ToList()),
                new ColumnModel("Study", _allPhaseInstances.Select(i => GetExperimentByPhase(i).ShortName).ToList()),
                new ColumnModel("Name", _allPhaseInstances.Select(i => GetExpRunByPhase(i).Name).ToList()),
                new ColumnModel("P [Kbar]", _allPhaseInstances.Select(i => GetExpRunByPhase(i).Pressure).ToList()),
                new ColumnModel("T [°C]", _allPhaseInstances.Select(i => GetExpRunByPhase(i).Temperature).ToList()),
                new ColumnModel("Phases",
                    _allPhaseInstances.Select(i => GetExpRunByPhase(i).GetPhaseAssemblageString()).ToList())
            };

            var allOxides = _allPhaseInstances.SelectMany(i => i.CompositionOxides).Distinct();
            foreach (var oxide in allOxides)
            {
                var values = _allPhaseInstances.Select(i =>
                    i.Composition.TryGetValue(oxide, out double value) ? value.Round(6) : 0).ToList();
                var columnModel = new ColumnModel(oxide.ToString(), values);
                columns.Add(columnModel);
            }
            columns.Add(new ColumnModel("Total", _allPhaseInstances.Select(i => i.CompositionValues.Sum().Round(6)).ToList()));

            return new TableModel(phaseItem.Label, columns) { IsReadOnly = true };
        }

        public TableModel CreatePhasesSummaryTableModel()
        {
            if (_allPhaseInstances == null || _allPhaseInstances.Count == 0) return null;

            var columns = new List<ColumnModel>
            {
                new ColumnModel("", new List<string>() { "Minimum", "Average", "Maximum" })
            };

            var allOxides = _allPhaseInstances.SelectMany(i => i.CompositionOxides).Distinct();
            foreach (var oxide in allOxides)
            {
                var allVals = _allPhaseInstances.Select(i =>
                    i.Composition.TryGetValue(oxide, out double value) ? value : 0).ToList();
                var colVals = new List<double> { allVals.Min(), allVals.Average(), allVals.Max() };
                columns.Add(new ColumnModel(oxide.ToString(), colVals.Select(x => x.Round(6)).ToList()));
            }
            var totals = _allPhaseInstances.Select(i => i.CompositionValues.Sum()).ToList();
            var lastColVals = new List<double> { totals.Min(), totals.Average(), totals.Max() };
            columns.Add(new ColumnModel("Total", lastColVals.Select(x => x.Round(6)).ToList()));

            return new TableModel("Summary", columns) { IsReadOnly = true, FrozenColumnCount = 1 };
        }

        private ExpRun GetExpRunByPhase(Phase phase)
        {
            return _experiments.SelectMany(e => e.Runs).FirstOrDefault(r => r.Id == phase.ExpRunId);
        }

        private Experiment GetExperimentByPhase(Phase phase)
        {
            var expRun = GetExpRunByPhase(phase);
            if (expRun == null) return null;
            return _experiments.FirstOrDefault(e => e.Runs.Contains(expRun));
        }

        public void DeleteExperiments(List<Guid> Ids)
        {
            foreach (var id in Ids)
            {
                var item = ExperimentItems.FirstOrDefault(x => x.PresenterId == id);
                if (item == null) continue;
                ExperimentItems.Remove(item);
            }
        }

        public ExpRun GetExpRunById(Guid id)
        {
            return _experiments.SelectMany(x => x.Runs).FirstOrDefault(r => r.Id == id);
        }
    }
}