using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xeovis.Domain;
using Xeovis.UI.Models;

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
                //todo: notify
            }
        }

        public void DeleteAllExperiments()
        {
            Experiments.Clear();
            ExperimentNodes.Clear();
            //todo: notify
        }

        public void DeleteMctcBundle(Guid Id)
        {
            var bundle = mctcBundles.FirstOrDefault(x => x.Id == Id);
            var node = MctcBundleNodes.FirstOrDefault(x => x.PresenterId == Id);
            if (node != null && bundle != null)
            {
                mctcBundles.Remove(bundle);
                MctcBundleNodes.Remove(node);
                //todo: notify
            }
        }

        public void DeleteAllMctcBundles()
        {
            mctcBundles.Clear();
            MctcBundleNodes.Clear();
            //todo: notify
        }

        public void DeleteMctcPlot(Guid Id)
        {
            var plotDto = mctcPlotDtos.FirstOrDefault(x => x.Id == Id);
            if (plotDto == null) return;
            var bundleNode = mctcBundleNodes.FirstOrDefault(x => x.PresenterId == plotDto.mctcBundleId);
            if (bundleNode == null) return;
            var plotNode = bundleNode.Children.FirstOrDefault(x => x.PresenterId == Id);
            bundleNode.Children.Remove(plotNode);
            // notify
        }

        public void DeleteAllMctcPlotsInBundle(Guid Id)
        {
            var plotDto = mctcPlotDtos.FirstOrDefault(x => x.Id == Id);
            if (plotDto == null) return;
            var bundleNode = mctcBundleNodes.FirstOrDefault(x => x.PresenterId == plotDto.mctcBundleId);
            if (bundleNode == null) return;
            bundleNode.Children.Clear();
            // notify
        }
    }
}
