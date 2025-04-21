using Avalonia.Controls;
using System.Collections.Generic;
using Xeovis.Domain;
using Xeovis.UI.Models;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel = new MainWindowViewModel();
        public MainWindowViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; }
        }

        private DataManagerTabControl? _dataManagerTab;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            PrepareUI();
        }

        private void MainWindow_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            mainMenu.ParentWindow = this;
            tabControl.ItemsSource = _viewModel.TabControls;

            var dataManagerViewModel = _viewModel.CreateDataManagerTabVM();
            _dataManagerTab = new DataManagerTabControl() { ViewModel = dataManagerViewModel };
            dataManagerViewModel.OwnerTabControl = _dataManagerTab;
            dataManagerViewModel.IsVisible = false;
        }

        private void PrepareUI()
        {
            mainMenu.ExperimentsLoaded += MainMenu_ExperimentsLoaded;
            mainMenu.mctcBundleLoaded += MainMenu_mctcBundleLoaded;
        }

        private void MainMenu_mctcBundleLoaded(object? sender, Models.Events.mctcBundleLoadedEventArgs e)
        {
            var bundle = e.mctcBundle;
            _viewModel.mctcBundles.Add(bundle);
            var mctcNode = new TreeNode(bundle.Name) { Tag = bundle.Id };
            tvMctcBundles.Items.Add(mctcNode);
        }

        private void MainMenu_ExperimentsLoaded(object? sender, Models.Events.ExperimentsLoadedEventArgs e)
        {
            tvDataManager.Items.Clear();
            _viewModel.Experiments = e.Experiments;
            foreach (var exp in e.Experiments)
            {
                var expNode = new TreeNode(exp.Name);
                foreach (var phase in exp.PhasesWithData)
                {
                    var phaseNode = new TreeNode(phase.ToString());
                    expNode.Children.Add(phaseNode);
                }

                tvDataManager.Items.Add(expNode);
            }

            if (_dataManagerTab != null)
            {
                _dataManagerTab.SetExperiments(e.Experiments);
                _dataManagerTab.ViewModel.IsVisible = e.Experiments.Count > 0;
            }
        }

        public void CreateMctcPlot(mctcPlotDto mctcPlotDto)
        {
            if (mctcPlotDto == null) return;

        }
    }
}