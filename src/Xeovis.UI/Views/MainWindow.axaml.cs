using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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
            tvMctcBundles.ItemsSource = _viewModel.MctcBundleNodes;
            mainMenu.ExperimentsLoaded += MainMenu_ExperimentsLoaded;
            mainMenu.mctcBundleLoaded += MainMenu_mctcBundleLoaded;
            //tabControl.ContextMenu = new ContextMenu();
        }

        private void MainMenu_mctcBundleLoaded(object? sender, Models.Events.mctcBundleLoadedEventArgs e)
        {
            var bundle = e.mctcBundle;
            _viewModel.mctcBundles.Add(bundle);
            var mctcNode = new TreeNode(bundle.Name) { PresenterId = bundle.Id };
            _viewModel.MctcBundleNodes.Add(mctcNode);
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

        private async void btnCloseTab_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is not Button button) return;
            if (button.Tag is not Guid Id) return;
            var tabViewModel = tabControl.Items.FirstOrDefault(x => (x as TabControlViewModelBase).Id == Id) as TabControlViewModelBase;


            var msgBox = MessageBoxManager.GetMessageBoxStandard("Warning", $"You are about to remove {tabViewModel.Header}.\nAre you sure you want to continue?", ButtonEnum.YesNoCancel, MsBox.Avalonia.Enums.Icon.Warning);
            var result = await msgBox.ShowWindowDialogAsync(this);
            if (result == ButtonResult.Yes)
            {
                _viewModel.TabControls.Remove(tabViewModel);

                TreeNode bundleNode = null;
                if (tabViewModel is mctcPlotSingleViewModel singleViewModel)
                    bundleNode = tvMctcBundles.Items.FirstOrDefault(x => (x as TreeNode).PresenterId == singleViewModel.mctcBundle.Id) as TreeNode;
                else if (tabViewModel is mctcPlotGridViewModel gridViewModel)
                    bundleNode = tvMctcBundles.Items.FirstOrDefault(x => (x as TreeNode).PresenterId == gridViewModel.mctcBundle.Id) as TreeNode;

                if (bundleNode == null) return;
                var plotNode = bundleNode.Children.FirstOrDefault(x => x.PresenterId == tabViewModel.PresentedId);
                bundleNode.Children.Remove(plotNode);
            }
        }

        #region Public Methods
        public void CreateMctcPlot(mctcPlotDto mctcPlotDto)
        {
            if (mctcPlotDto == null) return;
            var mctcBundle = _viewModel.mctcBundles.FirstOrDefault(b => b.Id == mctcPlotDto.mctcBundleId);
            if (mctcBundle == null) return;

            bool successful = false;
            TabControlViewModelBase viewModel;
            UserControl plotTab;
            if (mctcPlotDto.IsGridView && mctcPlotDto.SampleXeosVariables.Count > 1)
            {
                viewModel = new mctcPlotGridViewModel() { Header = mctcPlotDto.Name, PresentedId = mctcPlotDto.Id };
                plotTab = new mctcPlotGridViewTabControl() { ViewModel = viewModel as mctcPlotGridViewModel };
                successful = (plotTab as mctcPlotGridViewTabControl).PopulateUI(mctcPlotDto, mctcBundle);
            }
            else
            {
                viewModel = new mctcPlotSingleViewModel() { Header = mctcPlotDto.Name, PresentedId = mctcPlotDto.Id };
                plotTab = new mctcPlotSingleViewTabControl() { ViewModel = viewModel as mctcPlotSingleViewModel };
                successful = (plotTab as mctcPlotSingleViewTabControl).PopulateUI(mctcPlotDto, mctcBundle);
            }

            if (successful)
            {
                viewModel.OwnerTabControl = plotTab;
                viewModel.CanBeRemoved = true;
                _viewModel.TabControls.Add(viewModel);
                tabControl.SelectedIndex = tabControl.Items.IndexOf(viewModel);

                var plotNode = new TreeNode(mctcPlotDto.Name) { PresenterId = mctcPlotDto.Id };
                var mctcBundleNode = tvMctcBundles.Items.FirstOrDefault(x => (x as TreeNode).PresenterId == mctcBundle.Id) as TreeNode;
                if (mctcBundleNode != null)
                {
                    mctcBundleNode.Children.Add(plotNode);
                    mctcBundleNode.IsExpanded = true; //todo: bind to UI - M.H. 23/04/2025
                }
            }
        }

        public void ToggleLeftPanelVisibility(bool visible)
        {
            var column = mainGrid.ColumnDefinitions[0];
            var splitter = mainGrid.ColumnDefinitions[1];
            if (!visible)
            {
                column.MinWidth = 0;
                column.Width = new GridLength(0);

                splitter.MinWidth = 0;
                splitter.Width = new GridLength(0);
            }
            else
            {
                column.MinWidth = 150;
                column.Width = new GridLength(200);

                splitter.MinWidth = 5;
                splitter.Width = new GridLength(5);
            }

            borderTreeLists.IsVisible = visible;
            splitterVertical.IsVisible = visible;
        }

        #endregion
    }
}