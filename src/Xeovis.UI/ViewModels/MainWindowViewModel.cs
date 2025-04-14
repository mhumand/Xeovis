using CommunityToolkit.Mvvm.ComponentModel;
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
        private ObservableCollection<TreeNode> mctcPlotNodes;

        [ObservableProperty]
        private ObservableCollection<TabControlViewModelBase> tabControls;

        public List<Experiment> Experiments { get; set; }

        public MainWindowViewModel()
        {
            ExperimentNodes = new ObservableCollection<TreeNode>();
            mctcPlotNodes = new ObservableCollection<TreeNode>();
            TabControls = new ObservableCollection<TabControlViewModelBase>();
            Experiments = new List<Experiment>();
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
    }
}
