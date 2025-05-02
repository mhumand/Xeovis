using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xeovis.Domain;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Events;

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
        }

        public DataManagerTabControlViewModel CreateDataManagerTabVM()
        {
            var viewModel = new DataManagerTabControlViewModel(Experiments) { Header = "Data Manager" };
            TabControls.Add(viewModel);
            return viewModel;
        }

        public DataManagerTabControlViewModel? GetDataManagerTabVM()
        {
            if (TabControls == null || TabControls.Count == 0) return null;
            return TabControls.FirstOrDefault(t => t is DataManagerTabControlViewModel) as DataManagerTabControlViewModel;
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
            }
        }

        public void DeleteAllExperiments()
        {
            var idList = Experiments.Select(x => x.Id).ToList();
            Experiments.Clear();
            ExperimentNodes.Clear();
            ExperimentsDeleted?.Invoke(this, new ExperimentsDeletedEventArgs(idList));
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
            }
        }

        public void DeleteAllMctcBundles()
        {
            var idList = mctcBundles.Select(x => x.Id).ToList();
            mctcBundles.Clear();
            MctcBundleNodes.Clear();
            mctcBundlesDeleted?.Invoke(this, new mctcBundlesDeletedEventArgs(idList));
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
        }

        public void DeleteAllMctcPlotsInBundle(Guid Id)
        {
            var plotDto = mctcPlotDtos.FirstOrDefault(x => x.Id == Id);
            if (plotDto == null) return;
            var bundleNode = mctcBundleNodes.FirstOrDefault(x => x.PresenterId == plotDto.mctcBundleId);
            if (bundleNode == null) return;
            var idList = bundleNode.Children.Select(x => x.PresenterId).ToList();
            bundleNode.Children.Clear();
            mctcPlotsDeleted?.Invoke(this, new mctcPlotsDeletedEventArgs(idList));
        }
    }
}
