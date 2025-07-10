using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Serilog;
using Xeovis.Domain;
using Xeovis.Presentation;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Events;
using Xeovis.Persistence;

namespace Xeovis.UI.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<TreeNode> experimentNodes;

        [ObservableProperty]
        private ObservableCollection<TreeNode> mctcBundleNodes;

        [ObservableProperty]
        private ObservableCollection<TabControlViewModelBase> tabControls;

        public List<Experiment> Experiments { get; set; }
        public List<mctcBundle> mctcBundles { get; set; }
        public List<mctcPlotDto> mctcPlotDtos { get; set; }
        public ProjectSettings ProjectSettings { get; set; }

        public event EventHandler<ExperimentsDeletedEventArgs>? ExperimentsDeleted;
        public event EventHandler<mctcBundlesDeletedEventArgs>? mctcBundlesDeleted;
        public event EventHandler<mctcPlotsDeletedEventArgs>? mctcPlotsDeleted;

        public MainWindowViewModel()
        {
            ExperimentNodes = new ObservableCollection<TreeNode>();
            mctcBundleNodes = new ObservableCollection<TreeNode>();
            TabControls = new ObservableCollection<TabControlViewModelBase>();
            Experiments = new List<Experiment>();
            mctcBundles = new List<mctcBundle>();
            mctcPlotDtos = new List<mctcPlotDto>();
            ProjectSettings = new ProjectSettings();
        }

        public DataManagerTabControlViewModel CreateDataManagerTabVM()
        {
            var viewModel = new DataManagerTabControlViewModel(Experiments) { Header = "Data Manager" };
            TabControls.Insert(0, viewModel);
            return viewModel;
        }

        public DataManagerTabControlViewModel? GetDataManagerTabVM()
        {
            if (TabControls == null || TabControls.Count == 0) return null;
            return TabControls.FirstOrDefault(t => t is DataManagerTabControlViewModel) as DataManagerTabControlViewModel;
        }

        public void ClearAll()
        {
            ExperimentNodes.Clear();
            MctcBundleNodes.Clear();
            TabControls.Clear();
            Experiments.Clear();
            mctcBundles.Clear();
            mctcPlotDtos.Clear();
            ProjectSettings = new ProjectSettings();
        }

        public void DeleteExperiment(Guid Id)
        {
            var exp = Experiments.FirstOrDefault(x => x.Id == Id);
            var node = ExperimentNodes.FirstOrDefault(x => x.PresenterId == Id);
            if (exp != null && node != null)
            {
                Experiments.Remove(exp);
                ExperimentNodes.Remove(node);
                ExperimentsDeleted?.Invoke(this, new ExperimentsDeletedEventArgs(Id));
                Log.Information("Experiment deleted: {name}", exp.Name);
            }
        }

        public void DeleteAllExperiments()
        {
            var idList = Experiments.Select(x => x.Id).ToList();
            Experiments.Clear();
            ExperimentNodes.Clear();
            ExperimentsDeleted?.Invoke(this, new ExperimentsDeletedEventArgs(idList));
            Log.Information("All experiments deleted.");
        }

        public void DeleteMctcBundle(Guid Id)
        {
            var bundle = mctcBundles.FirstOrDefault(x => x.Id == Id);
            var node = MctcBundleNodes.FirstOrDefault(x => x.PresenterId == Id);
            if (node != null && bundle != null)
            {
                mctcBundles.Remove(bundle);
                MctcBundleNodes.Remove(node);
                mctcBundlesDeleted?.Invoke(this, new mctcBundlesDeletedEventArgs(Id));
                Log.Information("Bundle deleted: {name}", bundle.Name);
            }
        }

        public void DeleteAllMctcBundles()
        {
            var idList = mctcBundles.Select(x => x.Id).ToList();
            mctcBundles.Clear();
            MctcBundleNodes.Clear();
            mctcBundlesDeleted?.Invoke(this, new mctcBundlesDeletedEventArgs(idList));
            Log.Information("All bundles deleted.");
        }

        public void DeleteMctcPlot(Guid Id)
        {
            var plotDto = mctcPlotDtos.FirstOrDefault(x => x.Id == Id);
            if (plotDto == null) return;
            var bundleNode = mctcBundleNodes.FirstOrDefault(x => x.PresenterId == plotDto.mctcBundleId);
            if (bundleNode == null) return;
            var plotNode = bundleNode.Children.FirstOrDefault(x => x.PresenterId == Id);
            bundleNode.Children.Remove(plotNode);
            mctcPlotsDeleted?.Invoke(this, new mctcPlotsDeletedEventArgs(Id));
            Log.Information("Plot deleted: {name}", plotDto.Name);
        }

        public void DeleteAllMctcPlotsInBundle(Guid plotID)
        {
            var plotDto = mctcPlotDtos.FirstOrDefault(x => x.Id == plotID);
            if (plotDto == null) return;
            var bundleNode = mctcBundleNodes.FirstOrDefault(x => x.PresenterId == plotDto.mctcBundleId);
            if (bundleNode == null) return;
            var idList = bundleNode.Children.Select(x => x.PresenterId).ToList();
            bundleNode.Children.Clear();
            mctcPlotsDeleted?.Invoke(this, new mctcPlotsDeletedEventArgs(idList));
            Log.Information("All plots deleted in bundle: {name}}", plotDto.Name);
        }
    }
}
