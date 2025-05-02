using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xeovis.Base;
using Xeovis.Domain;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels
{
    public partial class DataManagerTabControlViewModel : TabControlViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<ListableItem> experimentItems;

        [ObservableProperty]
        private ObservableCollection<ListableItem> phaseInExperimentItems;

        private List<Experiment> _experiments;
        private List<Phase> _allPhaseInstances;
        public DataManagerTabControlViewModel()
        {
            ExperimentItems = new ObservableCollection<ListableItem>();
            PhaseInExperimentItems = new ObservableCollection<ListableItem>();

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
                ExperimentItems.Add(new ListableItem(experiment.Name, description: experiment.ShortName) { PresenterId = experiment.Id });
            }
        }

        public TableModel CreateExperimentTableModel(ListableItem experimentItem)
        {
            var exp = _experiments.FirstOrDefault(e => e.Id == experimentItem.PresenterId);
            if (exp == null || exp.Runs.Count == 0) return null;

            var columns = new List<ColumnModel>
            {
                new ColumnModel("#", exp.Runs.Select(r => r.Index.ToString("000")).ToList()),
                new ColumnModel("Name", exp.Runs.Select(r => r.Name).ToList()),
                new ColumnModel("P [Kbar]", exp.Runs.Select(r => r.Pressure).ToList()),
                new ColumnModel("T [°C]", exp.Runs.Select(r => r.Temperature).ToList()),
                new ColumnModel("t [hr]", exp.Runs.Select(r => r.Duration).ToList()),
                new ColumnModel("Rocktype", exp.Runs.Select(r => r.Rocktype.ToString()).ToList()),
                new ColumnModel("Phases", exp.Runs.Select(r => r.GetPhaseAssemblageString()).ToList())
            };

            int counter = 0;
            foreach (var oxide in exp.Runs[0].BulkOxides)
            {
                columns.Add(new ColumnModel(oxide.ToString(), exp.Runs.Select(r => r.BulkComposition[counter].Round(4)).ToList()));
                counter++;
            }

            return new TableModel(exp.Name, columns) { IsReadOnly = true, FrozenColumnCount = 2 };
        }

        public void CreatePhaseInExperimentItems(ListableItem experimentItem)
        {
            var exp = _experiments.FirstOrDefault(e => e.Id == experimentItem.PresenterId);
            if (exp == null) return;

            PhaseInExperimentItems.Clear();
            foreach (var phase in exp.PhasesWithData)
                PhaseInExperimentItems.Add(new ListableItem(EnumExtensions.GetDescriptionFromValue(phase), description: phase.ToString()));
        }

        public TableModel CreatePhaseInExperimentTableModel(ListableItem experimentItem, ListableItem phaseItem)
        {
            var exp = _experiments.FirstOrDefault(e => e.Id == experimentItem.PresenterId);
            if (exp == null || exp.Runs.Count == 0) return null;

            var phaseType = PhaseTypeExtensions.GetPhaseTypeByName(phaseItem.Label);

            var allInstances = new List<Phase>();
            foreach (var run in exp.Runs.Where(r => r.Phases.Any(ph => ph.Type == phaseType)))
                allInstances.Add(run.Phases.FirstOrDefault(ph => ph.Type == phaseType));

            allInstances.RemoveAll(i => !i.Composition.Any());
            if (!allInstances.Any()) return null;
            int maxOxideCount = allInstances.Max(i => i.Oxides.Count());
            var oxides = allInstances.FirstOrDefault(i => i.Oxides.Count() == maxOxideCount).Oxides;

            var columns = new List<ColumnModel>
            {
                new ColumnModel("Run", allInstances.Select(i => exp.Runs.FirstOrDefault(r => r.Id == i.ExpRunId).ToString()).ToList()),
            };

            int counter = 0;
            foreach (var oxide in oxides)
            {
                columns.Add(new ColumnModel(oxide.ToString(), allInstances.Select(i => i.Composition[counter].Round(8)).ToList()));
                counter++;
            }

            columns.Add(new ColumnModel("Comment", allInstances.Select(i => i.Comment).ToList()));

            return new TableModel(phaseItem.Label, columns) { IsReadOnly = true, FrozenColumnCount = 1 };
        }

        public List<ListableItem> GetPhaseTypeItemList()
        {
            var phases = _experiments.SelectMany(e => e.PhasesWithData).Distinct().ToList();
            var list = new List<ListableItem>();
            foreach (var phase in phases)
                list.Add(new ListableItem(EnumExtensions.GetDescriptionFromValue(phase), tag: phase, description: phase.ToString()));

            return list.OrderBy(l => l.Label).ToList();
        }

        public TableModel CreatePhasesTableModel(ListableItem phaseItem)
        {
            var phaseType = (PhaseType)phaseItem.Tag;

            _allPhaseInstances = _experiments.Where(e => e.PhasesWithData.Contains(phaseType)).SelectMany(e => e.Runs.Where(r => r.Phases.Select(p => p.Type).Contains(phaseType)).Select(r => r.Phases.FirstOrDefault(p => p.Type == phaseType && p.Composition.Any(x => x > 0)))).ToList();

            _allPhaseInstances.RemoveAll(p => p == null);
            if (_allPhaseInstances.Count == 0) return null;

            var columns = new List<ColumnModel>
            {
                new ColumnModel("#", Enumerable.Range(1, _allPhaseInstances.Count()).Select(x => x.ToString("000")).ToList()),
                new ColumnModel("Study", _allPhaseInstances.Select(i => GetExperiment(i).ShortName).ToList()),
                new ColumnModel("Name", _allPhaseInstances.Select(i => GetExpRun(i).Name).ToList()),
                new ColumnModel("P [Kbar]", _allPhaseInstances.Select(i => GetExpRun(i).Pressure).ToList()),
                new ColumnModel("T [°C]", _allPhaseInstances.Select(i => GetExpRun(i).Temperature).ToList()),
                new ColumnModel("Phases", _allPhaseInstances.Select(i => GetExpRun(i).GetPhaseAssemblageString()).ToList())
            };

            var oxides = Enum.GetValues(typeof(OxideType)).Cast<OxideType>().Except(new List<OxideType> { OxideType.None });
            foreach (var oxide in oxides)
            {
                columns.Add(new ColumnModel(oxide.ToString(), _allPhaseInstances.Select(i => i.Oxides.Contains(oxide) ? i.Composition[i.Oxides.IndexOf(oxide)].Round(4) : 0.0).ToList()));
            }

            return new TableModel(phaseItem.Label, columns) { IsReadOnly = true };
        }

        public TableModel CreatePhasesSummaryTableModel()
        {
            if (_allPhaseInstances == null || _allPhaseInstances.Count == 0) return null;

            var columns = new List<ColumnModel>
            {
                new ColumnModel("", new List<string>() { "Minimum", "Average", "Maximum" })
            };

            var oxides = Enum.GetValues(typeof(OxideType)).Cast<OxideType>().Except(new List<OxideType> { OxideType.None });
            foreach (var oxide in oxides)
            {
                var allVals = _allPhaseInstances.Select(i => i.Oxides.Contains(oxide) ? i.Composition[i.Oxides.IndexOf(oxide)].Round(4) : 0.0).ToList();
                var colVals = new List<double> { allVals.Min(), allVals.Average(), allVals.Max() };
                columns.Add(new ColumnModel(oxide.ToString(), colVals.Select(x => x.Round(4)).ToList()));
            }

            return new TableModel("Summary", columns) { IsReadOnly = true, FrozenColumnCount = 1 };
        }

        private ExpRun GetExpRun(Phase phase)
        {
            return _experiments.SelectMany(e => e.Runs).FirstOrDefault(r => r.Id == phase.ExpRunId);
        }

        private Experiment GetExperiment(Phase phase)
        {
            var expRun = GetExpRun(phase);
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
    }
}
