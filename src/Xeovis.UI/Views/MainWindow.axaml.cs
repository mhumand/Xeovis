using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Domain;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;
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
            tvExperiments.ItemsSource = _viewModel.ExperimentNodes;

            tvExperiments.ContextRequested += tvExperiments_ContextRequested;
            tvMctcBundles.ContextRequested += tvMctcBundles_ContextRequested;
            mainMenu.ExperimentsLoaded += MainMenu_ExperimentsLoaded;
            mainMenu.mctcBundleLoaded += MainMenu_mctcBundleLoaded;
        }

        #region Events
        private void MainMenu_mctcBundleLoaded(object? sender, Models.Events.mctcBundleLoadedEventArgs e)
        {
            var bundle = e.mctcBundle;
            _viewModel.mctcBundles.Add(bundle);
            var mctcNode = new TreeNode(bundle.Name) { PresenterId = bundle.Id };
            _viewModel.MctcBundleNodes.Add(mctcNode);
        }

        private void MainMenu_ExperimentsLoaded(object? sender, Models.Events.ExperimentsLoadedEventArgs e)
        {
            _viewModel.ExperimentNodes.Clear();
            _viewModel.Experiments = e.Experiments;
            foreach (var exp in e.Experiments)
            {
                var expNode = new TreeNode(exp.Name) { PresenterId = exp.Id };
                foreach (var phase in exp.PhasesWithData)
                {
                    var phaseNode = new TreeNode(phase.ToString()) { Tag = phase };
                    expNode.Children.Add(phaseNode);
                }

                _viewModel.ExperimentNodes.Add(expNode);
            }

            if (_dataManagerTab != null)
            {
                _dataManagerTab.SetExperiments(e.Experiments);
                _dataManagerTab.ViewModel.IsVisible = e.Experiments.Count > 0;
            }
        }

        private void tvMctcBundles_ContextRequested(object? sender, ContextRequestedEventArgs e)
        {
            if (sender is not TreeView treeView) return;
            if (treeView.SelectedItem is not TreeNode node) return;

            var menu = new ContextMenu();

            var renameItem = new MenuItem() { Header = "Rename" };
            menu.Items.Add(renameItem);

            menu.Items.Add(new Separator());

            var deleteItem = new MenuItem() { Header = "Delete" };
            menu.Items.Add(deleteItem);

            var deleteAllItem = new MenuItem() { Header = "Delete All" };
            menu.Items.Add(deleteAllItem);

            var isMctcBundle = _viewModel.mctcBundles.Any(x => x.Id == node.PresenterId);
            if (isMctcBundle)
            {
                var bundle = _viewModel.mctcBundles.FirstOrDefault(x => x.Id == node.PresenterId);

                var addPlotItem = new MenuItem() { Header = "Add New Plot" };
                addPlotItem.Click += (s, e) => mainMenu.LoadNewPlotDialog(node.PresenterId);
                menu.Items.Insert(0, addPlotItem);

                var reloadItem = new MenuItem() { Header = "Reload" };
                ToolTip.SetTip(reloadItem, "Reloads the bundle from its directory");
                reloadItem.Click += (s, e) => mainMenu.LoadMctcBundle(bundle.FilesDirectory);
                menu.Items.Insert(1, reloadItem);

                deleteItem.Click += (s, e) => RequestRemovalConfirmationAndProceed(() => _viewModel.DeleteMctcBundle(node.PresenterId), node.Label);
                deleteAllItem.Click += (s, e) => RequestRemovalConfirmationAndProceed(() => _viewModel.DeleteAllMctcBundles(), "all mctc bundles");

                if (node.HasChildren)
                {
                    menu.Items.Add(new Separator());
                    AddExpandCollapseItemsToMenu(menu, node, false);
                }

                // rename
                var otherNames = _viewModel.mctcBundles.Except(new List<mctcBundle> { bundle }).Select(x => x.Name).ToList();

                void Rename(mctcBundle mctcBundle, string newName)
                {
                    mctcBundle.Name = newName;
                    mctcBundleRenamed(mctcBundle);
                }

                renameItem.Click += (s, e) => RenameObjectWithDialog(bundle, otherNames, x => x.Name, Rename);
            }
            else
            {
                var tab = tabControl.Items.FirstOrDefault(x => (x as TabControlViewModelBase).PresentedId == node.PresenterId) as TabControlViewModelBase;
                var openItem = new MenuItem() { Header = "Open" };
                openItem.Click += (s, e) =>
                {
                    tab.IsVisible = true;
                    tabControl.SelectedItem = tab;
                };
                menu.Items.Insert(0, openItem);

                menu.Items.Add(new Separator());
                var visibleItem = new MenuItem() { Header = "Visible", IsChecked = tab.IsVisible, ToggleType = MenuItemToggleType.CheckBox };
                visibleItem.Click += (s, e) =>
                {
                    //visibleItem.IsChecked = !visibleItem.IsChecked;
                    tab.IsVisible = visibleItem.IsChecked;
                };
                menu.Items.Add(visibleItem);

                var mctcPlotDto = _viewModel.mctcPlotDtos.FirstOrDefault(x => x.Id == node.PresenterId);
                var bundle = _viewModel.mctcBundles.FirstOrDefault(x => x.Id == mctcPlotDto.mctcBundleId);

                deleteItem.Click += (s, e) => RequestRemovalConfirmationAndProceed(() => _viewModel.DeleteMctcPlot(node.PresenterId), $"{node.Label} in {bundle.Name}");
                deleteAllItem.Click += (s, e) => RequestRemovalConfirmationAndProceed(() => _viewModel.DeleteAllMctcPlotsInBundle(node.PresenterId), $"all plots in {bundle.Name}");

                // rename
                var otherNames = _viewModel.mctcPlotDtos.Except(new List<mctcPlotDto> { mctcPlotDto }).Select(x => x.Name).ToList();

                void Rename(mctcPlotDto plotDto, string newName)
                {
                    plotDto.Name = newName;
                    mctcPlotRenamed(plotDto);
                }

                renameItem.Click += (s, e) => RenameObjectWithDialog(mctcPlotDto, otherNames, x => x.Name, Rename);
            }

            menu.Items.Add(new Separator());

            var settingsItem = new MenuItem() { Header = "Settings" };
            menu.Items.Add(settingsItem);

            tvMctcBundles.ContextMenu = menu;
        }

        private void tvExperiments_ContextRequested(object? sender, ContextRequestedEventArgs e)
        {
            if (sender is not TreeView treeView) return;
            if (treeView.SelectedItem is not TreeNode node) return;

            var menu = new ContextMenu();

            var openItem = new MenuItem() { Header = "Open" };
            menu.Items.Add(openItem);

            menu.Items.Add(new Separator());

            var isExperiment = _viewModel.Experiments.Any(x => x.Id == node.PresenterId);
            if (isExperiment)
            {
                var deleteItem = new MenuItem() { Header = "Delete" };
                deleteItem.Click += (s, e) => RequestRemovalConfirmationAndProceed(() => _viewModel.DeleteExperiment(node.PresenterId), node.Label);
                menu.Items.Add(deleteItem);

                var deleteAllItem = new MenuItem() { Header = "Delete All" };
                deleteAllItem.Click += (s, e) => RequestRemovalConfirmationAndProceed(() => _viewModel.DeleteAllExperiments(), "all experiments");
                menu.Items.Add(deleteAllItem);

                if (node.HasChildren)
                {
                    menu.Items.Add(new Separator());
                    AddExpandCollapseItemsToMenu(menu, node, true);
                }
            }

            tvExperiments.ContextMenu = menu;
        }

        public async void RequestRemovalConfirmationAndProceed(Action action, string tobeRemovedName)
        {
            var message = $"You are about to remove {tobeRemovedName}.\nAre you sure you want to continue?";
            var dialog = MessageBoxManager.GetMessageBoxStandard("Confirmation", message, ButtonEnum.YesNoCancel, MsBox.Avalonia.Enums.Icon.Warning);
            if (await dialog.ShowWindowDialogAsync(this) == ButtonResult.Yes)
                action();
        }

        public async void RenameObjectWithDialog<T>(T target, List<string> existingNames, Func<T, string> getName, Action<T, string> setName)
        {
            string currentName = getName(target);

            var dialog = new RenameDialog(currentName, existingNames);
            var result = await dialog.ShowDialog<DialogResultType>(this);

            if (result == DialogResultType.OK && dialog.NewName != currentName)
            {
                setName(target, dialog.NewName);
            }
        }

        private void AddExpandCollapseItemsToMenu(ContextMenu menu, TreeNode node, bool isExperimentNode)
        {
            if (node.IsExpanded)
            {
                var collapseItem = new MenuItem() { Header = "Collapse" };
                collapseItem.Click += (s, e) => node.IsExpanded = false;
                menu.Items.Add(collapseItem);
            }
            else
            {
                var expandItem = new MenuItem() { Header = "Expand" };
                expandItem.Click += (s, e) => node.IsExpanded = true;
                menu.Items.Add(expandItem);
            }

            var expandAllItem = new MenuItem() { Header = "Expand All" };
            expandAllItem.Click += (s, e) =>
            {
                if (isExperimentNode)
                    _viewModel.ExperimentNodes.ToList().ForEach(x => x.IsExpanded = true);
                else
                    _viewModel.MctcBundleNodes.ToList().ForEach(x => x.IsExpanded = true);
            };

            menu.Items.Add(expandAllItem);

            var collapseAllItem = new MenuItem() { Header = "Collapse All" };
            collapseAllItem.Click += (s, e) =>
            {
                if (isExperimentNode)
                    _viewModel.ExperimentNodes.ToList().ForEach(x => x.IsExpanded = false);
                else
                    _viewModel.MctcBundleNodes.ToList().ForEach(x => x.IsExpanded = false);
            };

            menu.Items.Add(collapseAllItem);
        }

        private void mctcBundleRenamed(mctcBundle bundle)
        {
            var node = _viewModel.MctcBundleNodes.FirstOrDefault(x => x.PresenterId == bundle.Id);
            if (node != null)
                node.Label = bundle.Name;
        }

        private void mctcPlotRenamed(mctcPlotDto plot)
        {
            var bundleNode = _viewModel.MctcBundleNodes.FirstOrDefault(x => x.PresenterId == plot.mctcBundleId);
            if (bundleNode == null) return;
            var plotNode = bundleNode.Children.FirstOrDefault(x => x.PresenterId == plot.Id);
            var tab = tabControl.Items.FirstOrDefault(x => (x as TabControlViewModelBase).PresentedId == plot.Id) as TabControlViewModelBase;
            if (plotNode != null && tab != null)
            {
                plotNode.Label = plot.Name;
                tab.Header = plot.Name;
            }
        }
        #endregion

        private void btnCloseTab_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is not Button button) return;
            if (button.Tag is not Guid Id) return;
            var tabViewModel = tabControl.Items.FirstOrDefault(x => (x as TabControlViewModelBase).Id == Id) as TabControlViewModelBase;

            TreeNode bundleNode = null;
            if (tabViewModel is mctcPlotSingleViewModel singleViewModel)
                bundleNode = tvMctcBundles.Items.FirstOrDefault(x => (x as TreeNode).PresenterId == singleViewModel.mctcBundle.Id) as TreeNode;
            else if (tabViewModel is mctcPlotGridViewModel gridViewModel)
                bundleNode = tvMctcBundles.Items.FirstOrDefault(x => (x as TreeNode).PresenterId == gridViewModel.mctcBundle.Id) as TreeNode;

            if (bundleNode == null) return;

            void action()
            {
                _viewModel.TabControls.Remove(tabViewModel);
                var plotNode = bundleNode.Children.FirstOrDefault(x => x.PresenterId == tabViewModel.PresentedId);
                bundleNode.Children.Remove(plotNode);
            }

            RequestRemovalConfirmationAndProceed(action, $"{tabViewModel.Header} in {bundleNode.Label}");
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
                _viewModel.mctcPlotDtos.Add(mctcPlotDto);
                tabControl.SelectedIndex = tabControl.Items.IndexOf(viewModel);

                var plotNode = new TreeNode(mctcPlotDto.Name) { PresenterId = mctcPlotDto.Id };
                var mctcBundleNode = tvMctcBundles.Items.FirstOrDefault(x => (x as TreeNode).PresenterId == mctcBundle.Id) as TreeNode;
                if (mctcBundleNode != null)
                {
                    mctcBundleNode.Children.Add(plotNode);
                    mctcBundleNode.IsExpanded = true;
                    tvMctcBundles.SelectedItem = plotNode;
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