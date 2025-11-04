using Avalonia.Controls;
using Avalonia.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xeovis.Domain;
using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Defaults;
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Domain.Definitions.Globals;
using Xeovis.Presentation;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.ViewModels;
using Xeovis.UI.ViewModels.DialogsViewModels;
using Xeovis.UI.Views.Helpers;

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
            InitializeDefaultValues();
            Loaded += MainWindow_Loaded;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            PhaseNameList.PhaseNameListChanged += PhaseNameList_PhaseNameListChanged;
            xeosVariableNameList.xeosVariableNameListChanged += XeosVariableNameList_xeosVariableNameListChanged;
            PhaseOxideList.PhaseOxideListChanged += PhaseOxideList_PhaseOxideListChanged;
            xeosVariableCationList.xeosVariableCationListChanged += XeosVariableCationList_xeosVariableCationListChanged;

            PrepareUI();
        }

        private void InitializeDefaultValues()
        {
            DefaultPhaseNames.Initialize();
            PhaseNameList.Initialize();
            PhaseOxideList.Initialize();

            DefaultXeosVariableNames.Initialize();
            xeosVariableNameList.Initialize();
            xeosVariableCationList.Initialize();
        }

        private void XeosVariableNameList_xeosVariableNameListChanged(object? sender, xeosVariableNameListChangedEventArgs e)
        {
            _viewModel.ProjectSettings.PhaseXeosVariables = e.xeosVariableNameList;
            RefreshAllTabControls();
        }

        private void PhaseNameList_PhaseNameListChanged(object? sender, Domain.Definitions.Globals.PhaseNameListChangedEventArgs e)
        {
            _viewModel.ProjectSettings.AllowedPhaseNames = e.NameSets;
            RefreshAllTabControls();
        }

        private void PhaseOxideList_PhaseOxideListChanged(object? sender, PhaseOxideListChangedEventArgs e)
        {
            _viewModel.ProjectSettings.PhaseOxides = e.PhaseOxideList;
            RefreshAllTabControls();
        }

        private void XeosVariableCationList_xeosVariableCationListChanged(object? sender, xeosVariableCationListChangedEventArgs e)
        {
            _viewModel.ProjectSettings.xeosVariableCations = e.CationList;
            RefreshAllTabControls();
        }

        private void RefreshAllTabControls()
        {
            foreach (var tab in _viewModel.TabControls)
            {
                if (tab == null) continue;
                var owner = tab.OwnerTabControl;
                if (owner is mctcPlotSingleViewTabControl mctcSingle)
                    mctcSingle.Refresh();
                else if (owner is mctcPlotGridViewTabControl mctcGrid)
                    mctcGrid.Refresh();
                else if (owner is DataManagerTabControl dataManager)
                    dataManager.Refresh();
            }
        }

        private void MainWindow_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            mainMenu.ParentWindow = this;
            tabControl.ItemsSource = _viewModel.TabControls;
        }

        private void PrepareUI()
        {
            DragDrop.SetAllowDrop(this, true);
            AddHandler(DragDrop.DropEvent, Drop);
            AddHandler(DragDrop.DragOverEvent, DragOver);
            this.Closing += MainWindow_Closing;

            tvMctcBundles.ItemsSource = _viewModel.MctcBundleNodes;
            tvExperiments.ItemsSource = _viewModel.ExperimentNodes;

            tvExperiments.ContextRequested += tvExperiments_ContextRequested;
            tvMctcBundles.ContextRequested += tvMctcBundles_ContextRequested;

            mainMenu.ExperimentsLoaded += MainMenu_ExperimentsLoaded;
            mainMenu.mctcBundleLoaded += MainMenu_mctcBundleLoaded;
            mainMenu.ProjectLoaded += MainMenu_ProjectLoaded;

            _viewModel.ExperimentsDeleted += _viewModel_ExperimentsDeleted;
            _viewModel.mctcBundlesDeleted += _viewModel_mctcBundlesDeleted;
            _viewModel.mctcPlotsDeleted += _viewModel_mctcPlotsDeleted;

            tabControl.ContextRequested += TabControl_ContextRequested;
            tabControl.SelectionChanged += TabControl_SelectionChanged;
        }

        private void TabControl_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is TabControlViewModelBase tabVMbase)
            {
                if (tabVMbase is DataManagerTabControlViewModel)
                {
                    tvMctcBundles.SelectedItem = null;
                    return;
                }

                var mctcPlotID = tabVMbase.PresenterId;
                var allNodes = tvMctcBundles.Items.Cast<TreeNode>().SelectMany(x => x.Children).ToList();
                var plotNode = allNodes.FirstOrDefault(x => x.PresenterId == mctcPlotID);
                tvMctcBundles.SelectedItem = plotNode;
                _viewModel.MctcBundleNodes.ToList().ForEach(x => x.IsExpanded = false);
                var bundleNode = _viewModel.MctcBundleNodes.FirstOrDefault(x => x.HasChildren && x.Children.Contains(plotNode));
                if (bundleNode != null) bundleNode.IsExpanded = true;
            }
        }

        private void TabControl_ContextRequested(object? sender, ContextRequestedEventArgs e)
        {
            if (e.Source is TextBlock txtblock && txtblock.Tag is TabControlViewModelBase tabViewModel)
            {

            }
        }

        #region Events
        private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
            _ = mainMenu.File_Quit();
            e.Cancel = true;
        }

        private void DragOver(object? sender, DragEventArgs e)
        {
            e.DragEffects = IsDraggedObjectValid(e, out _) ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }

        private void Drop(object? sender, DragEventArgs e)
        {
            if (!IsDraggedObjectValid(e, out string absolutePath)) return;
            var path = Uri.UnescapeDataString(absolutePath);
            mainMenu.LoadProjectFile(path, raiseUnsavedChanges: true);
        }

        private bool IsDraggedObjectValid(DragEventArgs e, out string absolutePath)
        {
            if (e.Data.Contains(DataFormats.Files))
            {
                var files = e.Data.GetFiles()?.ToList();
                if (files != null && files.Count == 1)
                {
                    var file = files.First();
                    if (Path.GetExtension(file.Path.AbsolutePath).Equals(".xvs", StringComparison.OrdinalIgnoreCase))
                    {
                        absolutePath = file.Path.AbsolutePath;
                        return true;
                    }
                }
            }

            absolutePath = string.Empty;
            return false;
        }

        private void MainMenu_ProjectLoaded(object? sender, Models.Events.ProjectFileLoadedEventArgs e)
        {
            _viewModel.ClearAll();
            _dataManagerTab = null;

            if (e.Project == null)
            {
                InitializeDefaultValues();
                this.Title = "Xeovis";
                return;
            };

            var settings = e.Project.ProjectSettings;
            _viewModel.ProjectSettings = settings;
            PhaseNameList.SetPhaseNames(settings.AllowedPhaseNames, false);
            xeosVariableNameList.SetXeosVariableNames(settings.PhaseXeosVariables, false);
            PhaseOxideList.SetPhaseOxides(settings.PhaseOxides, false);
            xeosVariableCationList.SetXeosVariableCations(settings.xeosVariableCations, false);

            var extension = Path.GetExtension(e.Path);
            this.Title = string.IsNullOrWhiteSpace(e.Project.ProjectName) ? "Xeovis" : $"Xeovis | {e.Project.ProjectName}{extension}";
            OnExperimentsLoaded(e.Project.Experiments);
            e.Project.mctcBundles.ForEach(x => OnMctcBundleLoaded(x));
            e.Project.mctcPlotDtos.ForEach(x => CreateMctcPlot(x));

            _viewModel.MctcBundleNodes.ToList().ForEach(x => x.IsExpanded = false);
            if (_viewModel.MctcBundleNodes.Count > 0)
                _viewModel.MctcBundleNodes.Last().IsExpanded = true;
        }

        private void MainMenu_mctcBundleLoaded(object? sender, Models.Events.mctcBundleLoadedEventArgs e)
        {
            OnMctcBundleLoaded(e.mctcBundle);
        }

        private void OnMctcBundleLoaded(mctcBundle bundle)
        {
            _viewModel.AssignColorToBundle(bundle);
            _viewModel.mctcBundles.Add(bundle);
            var mctcNode = new TreeNode(bundle.Name) { PresenterId = bundle.Id };
            mctcNode.BorderColor = GraphicsGenerator.GetBrushFromColor(bundle.Color);
            _viewModel.MctcBundleNodes.Add(mctcNode);
            Log.Information("Bundle loaded: {name}", bundle.Name);
        }

        private void MainMenu_ExperimentsLoaded(object? sender, Models.Events.ExperimentsLoadedEventArgs e)
        {
            OnExperimentsLoaded(e.Experiments);
        }

        private void OnExperimentsLoaded(List<Experiment> experiments)
        {
            _viewModel.ExperimentNodes.Clear();
            _viewModel.Experiments = experiments;
            foreach (var exp in experiments)
            {
                var expNode = new TreeNode(exp.Name) { PresenterId = exp.Id };
                foreach (var phase in exp.PhasesWithData)
                {
                    var phaseNode = new TreeNode(phase.ToString()) { PresenterId = exp.Id, Tag = phase };
                    expNode.Children.Add(phaseNode);
                }

                _viewModel.ExperimentNodes.Add(expNode);
            }

            if (_dataManagerTab == null)
            {
                var dataManagerViewModel = _viewModel.CreateDataManagerTabVM();
                _dataManagerTab = new DataManagerTabControl() { ViewModel = dataManagerViewModel };
                dataManagerViewModel.OwnerTabControl = _dataManagerTab;
                dataManagerViewModel.IsVisible = false;
            }

            _dataManagerTab.SetExperiments(experiments);
            _dataManagerTab.ViewModel.IsVisible = experiments.Count > 0;
            Log.Information("{ExperimentsCount} experiments loaded.", experiments.Count);
        }

        private void tvMctcBundles_ContextRequested(object? sender, ContextRequestedEventArgs e)
        {
            if (_viewModel.mctcBundles.Count == 0)
            {
                e.Handled = true;
                return;
            }

            if (sender is not TreeView treeView || treeView.SelectedItem is not TreeNode node)
            {
                e.Handled = true;
                return;
            }

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
                if (bundle == null)
                {
                    e.Handled = true;
                    return;
                }
                
                var addPlotItem = new MenuItem() { Header = "Add New Plot" };
                addPlotItem.Click += (s, e) => mainMenu.LoadNewPlotDialog(node.PresenterId);
                menu.Items.Insert(0, addPlotItem);

                var reloadItem = new MenuItem() { Header = "Reload" };
                ToolTip.SetTip(reloadItem, $"Reloads the bundle's contents from its directory:\n{bundle.FilesDirectory}");
                reloadItem.Click += reloadItem_Click;
                reloadItem.Tag = bundle;
                menu.Items.Insert(1, reloadItem);

                var reloadAndAppendItem = new MenuItem() { Header = "Reload & Append" };
                ToolTip.SetTip(reloadAndAppendItem, "Reloads the bundle’s contents from its directory and appends them to the existing content.");
                reloadAndAppendItem.Click += reloadAndAppendItem_Click; ;
                reloadAndAppendItem.Tag = bundle;
                menu.Items.Insert(2, reloadAndAppendItem);

                deleteItem.Click += (s, e) => RequestRemovalConfirmationAndProceed(() => _viewModel.DeleteMctcBundle(node.PresenterId), $"{node.Label} and its plots");
                deleteAllItem.Click += (s, e) => RequestRemovalConfirmationAndProceed(() => _viewModel.DeleteAllMctcBundles(), "all mctc bundles and their plots");

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
                    OnMctcBundleRenamed(mctcBundle);
                }
                renameItem.Click += (s, e) => RenameObjectWithDialog(bundle, otherNames, x => x.Name, Rename);

                // Directory
                if (DirectoryHelper.DirectoryExists(bundle.FilesDirectory))
                {
                    menu.Items.Add(new Separator());
                    var directoryItem = new MenuItem() { Header = "Go to Directory" };
                    directoryItem.Click += (s, e) => DirectoryHelper.GoToDirectory(bundle.FilesDirectory);
                    menu.Items.Add(directoryItem);
                }
            }
            else
            {
                var mctcPlotDto = _viewModel.mctcPlotDtos.FirstOrDefault(x => x.Id == node.PresenterId);
                var bundle = _viewModel.mctcBundles.FirstOrDefault(x => x.Id == mctcPlotDto.mctcBundleId);
                var tab = tabControl.Items.FirstOrDefault(x => (x as TabControlViewModelBase).PresenterId == node.PresenterId) as TabControlViewModelBase;

                if (mctcPlotDto == null || bundle == null || tab == null)
                {
                    e.Handled = true;
                    return;
                }

                var openItem = new MenuItem() { Header = "Open" };
                openItem.Click += (s, e) =>
                {
                    tab.IsVisible = true;
                    mctcPlotDto.IsVisible = true;
                    tabControl.SelectedItem = tab;
                };
                menu.Items.Insert(0, openItem);

                menu.Items.Add(new Separator());
                var visibleItem = new MenuItem() { Header = "Visible", IsChecked = tab.IsVisible, ToggleType = MenuItemToggleType.CheckBox };
                visibleItem.Click += (s, e) =>
                {
                    //visibleItem.IsChecked = !visibleItem.IsChecked;
                    tab.IsVisible = visibleItem.IsChecked;
                    mctcPlotDto.IsVisible = visibleItem.IsChecked;
                };
                menu.Items.Add(visibleItem);

                deleteItem.Click += (s, e) => RequestRemovalConfirmationAndProceed(() => _viewModel.DeleteMctcPlot(node.PresenterId), $"{node.Label} in {bundle.Name}");
                deleteAllItem.Click += (s, e) => RequestRemovalConfirmationAndProceed(() => _viewModel.DeleteAllMctcPlotsInBundle(node.PresenterId), $"all plots in {bundle.Name}");

                // rename
                var otherNames = _viewModel.mctcPlotDtos.Except(new List<mctcPlotDto> { mctcPlotDto }).Select(x => x.Name).ToList();
                void Rename(mctcPlotDto plotDto, string newName)
                {
                    plotDto.Name = newName;
                    OnMctcPlotRenamed(plotDto);
                }
                renameItem.Click += (s, e) => RenameObjectWithDialog(mctcPlotDto, otherNames, x => x.Name, Rename);
            }

            menu.Items.Add(new Separator());

            var settingsItem = new MenuItem() { Header = "Settings" };
            if (isMctcBundle)
            {
                // todo
                settingsItem.IsEnabled = false;
                //settingsItem.Click += (s, e)
            }
            else
                settingsItem.Click += (s, e) => ShowMctcPlotSettingsDialog(node.PresenterId);

            menu.Items.Add(settingsItem);

            tvMctcBundles.ContextMenu = menu;
            tvMctcBundles.ContextMenu.Open();
        }

        private void tvExperiments_ContextRequested(object? sender, ContextRequestedEventArgs e)
        {
            if (_viewModel.Experiments.Count == 0)
            {
                e.Handled = true;
                return;
            }

            if (sender is not TreeView treeView || treeView.SelectedItem is not TreeNode node)
            {
                e.Handled = true;
                return;
            }

            var menu = new ContextMenu();

            var openItem = new MenuItem() { Header = "Open" };
            menu.Items.Add(openItem);

            var isExperiment = node.Tag is not PhaseNameSet && _viewModel.Experiments.Any(x => x.Id == node.PresenterId);
            if (isExperiment)
            {
                var experiment = _viewModel.Experiments.FirstOrDefault(x => x.Id == node.PresenterId);

                menu.Items.Add(new Separator());
                openItem.Click += (s, e) => OpenExperiment(node.PresenterId);

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

                // Directory
                if (DirectoryHelper.DirectoryExists(experiment.Path))
                {
                    menu.Items.Add(new Separator());
                    var directoryItem = new MenuItem() { Header = "Go to Directory" };
                    directoryItem.Click += (s, e) => DirectoryHelper.GoToDirectory(experiment.Path);
                    menu.Items.Add(directoryItem);
                }
            }
            else
            {
                openItem.Click += (s, e) => OpenPhaseInExperiment(node.PresenterId, (PhaseNameSet)node.Tag);
            }

            tvExperiments.ContextMenu = menu;
            tvExperiments.ContextMenu.Open();
        }

        private async void reloadItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem) return;
            if (menuItem.Tag is not mctcBundle bundle) return;

            var newBundle = await mainMenu.LoadMctcBundle(bundle.FilesDirectory);
            if (newBundle != null)
            {
                newBundle.Id = bundle.Id;
                newBundle.Name = bundle.Name;
                var index = _viewModel.mctcBundles.FindIndex(x => x.Id == newBundle.Id);
                _viewModel.mctcBundles[index] = newBundle;

                foreach (var plotDto in _viewModel.mctcPlotDtos.Where(x => x.mctcBundleId == newBundle.Id))
                {
                    var tab = _viewModel.TabControls.FirstOrDefault(x => x.PresenterId == plotDto.Id);
                    if (tab?.OwnerTabControl is ImctcPlotView mctcPlot)
                        mctcPlot.PopulateUI(plotDto, newBundle);
                }

                Log.Information("Bundle reloaded: {name}", bundle.Name);
            }
            else
            {
                Log.Error("Failed to reload mctc bundle: {name}", bundle.Name);
                var mb = MessageBoxManager.GetMessageBoxStandard("Error", $"Failed to reload {bundle.Name}!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await mb.ShowWindowDialogAsync(this);
            }
        }

        private async void reloadAndAppendItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem) return;
            if (menuItem.Tag is not mctcBundle bundle) return;

            var newBundle = await mainMenu.LoadMctcBundle(bundle.FilesDirectory);
            if (newBundle != null)
            {
                foreach (var newBatch in newBundle.Batches)
                {
                    var oldBatch = bundle.Batches.FirstOrDefault(x => x.Name.Equals(newBatch.Name)
                    && newBatch.PhaseNameIds.SequenceEqual(x.PhaseNameIds));
                    if (oldBatch == null)
                        bundle.Batches.Add(newBatch);
                    else 
                    {
                        // overwrites the xeos var in the old batch
                        var vars = newBundle.xeosVariables.Where(x => x.BatchId == newBatch.Id).ToList();
                        vars.ForEach(x => x.BatchId = oldBatch.Id);
                        oldBatch.xeosVariables = vars;
                    }
                }

                foreach (var plotDto in _viewModel.mctcPlotDtos.Where(x => x.mctcBundleId == bundle.Id))
                {
                    var tab = _viewModel.TabControls.FirstOrDefault(x => x.PresenterId == plotDto.Id);
                    if (tab?.OwnerTabControl is ImctcPlotView mctcPlot)
                        mctcPlot.PopulateUI(plotDto, bundle);
                }

                Log.Information("Bundle reloaded: {name}", bundle.Name);
            }
            else
            {
                Log.Error("Failed to reload mctc bundle: {name}", bundle.Name);
                var mb = MessageBoxManager.GetMessageBoxStandard("Error", $"Failed to reload {bundle.Name}!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await mb.ShowWindowDialogAsync(this);
            }
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

            if ((isExperimentNode && _viewModel.ExperimentNodes.Any(x => !x.IsExpanded)) ||
                (!isExperimentNode && _viewModel.MctcBundleNodes.Any(x => !x.IsExpanded)))
            {
                var expandAllItem = new MenuItem() { Header = "Expand All" };
                expandAllItem.Click += (s, e) =>
                {
                    if (isExperimentNode)
                        _viewModel.ExperimentNodes.ToList().ForEach(x => x.IsExpanded = true);
                    else
                        _viewModel.MctcBundleNodes.ToList().ForEach(x => x.IsExpanded = true);
                };

                menu.Items.Add(expandAllItem);
            }

            if ((isExperimentNode && _viewModel.ExperimentNodes.Any(x => x.IsExpanded)) ||
                (!isExperimentNode && _viewModel.MctcBundleNodes.Any(x => x.IsExpanded)))
            {
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
        }

        private void OnMctcBundleRenamed(mctcBundle bundle)
        {
            var node = _viewModel.MctcBundleNodes.FirstOrDefault(x => x.PresenterId == bundle.Id);
            if (node == null) return;
            var oldName = node.Label;
            if (oldName == bundle.Name) return;
            node.Label = bundle.Name;
            Log.Information("Bundle renamed: {old} -> {new}", oldName, bundle.Name);
        }

        public void OnMctcPlotRenamed(mctcPlotDto plot)
        {
            var bundleNode = _viewModel.MctcBundleNodes.FirstOrDefault(x => x.PresenterId == plot.mctcBundleId);
            if (bundleNode == null) return;
            var plotNode = bundleNode.Children.FirstOrDefault(x => x.PresenterId == plot.Id);
            var tab = tabControl.Items.FirstOrDefault(x => (x as TabControlViewModelBase).PresenterId == plot.Id) as TabControlViewModelBase;
            if (plotNode == null || tab == null) return;
            var oldName = plotNode.Label;
            if (oldName == plot.Name) return;
            plotNode.Label = plot.Name;
            tab.Header = plot.Name;
            Log.Information("Plot renamed: {old} -> {new}", oldName, plot.Name);
        }

        private void _viewModel_mctcPlotsDeleted(object? sender, Models.Events.mctcPlotsDeletedEventArgs e)
        {
            foreach (var id in e.PlotIds)
            {
                var tab = GetTabById(id);
                if (tab == null) continue;
                _viewModel.TabControls.Remove(tab);
            }
        }

        private void _viewModel_mctcBundlesDeleted(object? sender, Models.Events.mctcBundlesDeletedEventArgs e)
        {
            foreach (var id in e.BundleIds)
            {
                var plots = _viewModel.mctcPlotDtos.Where(x => x.mctcBundleId == id);
                foreach (var plot in plots)
                {
                    var tab = GetTabById(plot.Id);
                    if (tab == null) continue;
                    _viewModel.TabControls.Remove(tab);
                }
            }
        }

        private void _viewModel_ExperimentsDeleted(object? sender, Models.Events.ExperimentsDeletedEventArgs e)
        {
            if (_dataManagerTab == null) return;
            _dataManagerTab.DeleteExperiments(e.ExperimentIds);
            _dataManagerTab.ViewModel.IsVisible = _viewModel.Experiments.Count > 0;
        }

        private async void ShowMctcPlotSettingsDialog(Guid Id)
        {
            var mctcPlotDto = _viewModel.mctcPlotDtos.FirstOrDefault(x => x.Id == Id);
            if (mctcPlotDto == null) return;
            var otherNames = _viewModel.mctcPlotDtos.Except(new List<mctcPlotDto> { mctcPlotDto }).Select(x => x.Name).ToList();
            var viewModel = new mctcPlotSettingsViewModel(mctcPlotDto, otherNames);
            var dialog = new mctcPlotSettingsDialog();
            dialog.ViewModel = viewModel;
            var result = await dialog.ShowDialog<DialogResultType>(this);
            if (result == DialogResultType.OK)
            {
                OnMctcPlotRenamed(mctcPlotDto);
                var tabVM = _viewModel.TabControls.FirstOrDefault(x => x.PresenterId == Id);
                var bundle = _viewModel.mctcBundles.FirstOrDefault(x => x.Id == mctcPlotDto.mctcBundleId);
                if (tabVM != null && bundle != null)
                    (tabVM.OwnerTabControl as ImctcPlotView).PopulateUI(mctcPlotDto, bundle);
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

            void DeleteMctcPlot()
            {
                _viewModel.TabControls.Remove(tabViewModel);
                var plotNode = bundleNode.Children.FirstOrDefault(x => x.PresenterId == tabViewModel.PresenterId);
                bundleNode.Children.Remove(plotNode);

                var plot = _viewModel.mctcPlotDtos.FirstOrDefault(x => x.Id == plotNode.PresenterId);
                Log.Information("Plot deleted: {name}", plot.Name);
            }

            RequestRemovalConfirmationAndProceed(DeleteMctcPlot, $"{tabViewModel.Header} in {bundleNode.Label}");
        }

        private TabControlViewModelBase GetTabById(Guid id)
        {
            return tabControl.Items.FirstOrDefault(x => (x as TabControlViewModelBase).PresenterId == id) as TabControlViewModelBase;
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
                viewModel = new mctcPlotGridViewModel() { Header = mctcPlotDto.Name, PresenterId = mctcPlotDto.Id };
                var tabPad = tabControl.Padding;
                var tabBounds = tabControl.Bounds;
                plotTab = new mctcPlotGridViewTabControl()
                {
                    ViewModel = viewModel as mctcPlotGridViewModel,
                    ParentWindow = this,
                    InitialBounds = new Avalonia.Rect(tabBounds.TopLeft.X - tabPad.Left,
                    tabBounds.TopLeft.Y - tabPad.Top, tabBounds.Width - tabPad.Left - tabPad.Right,
                    tabBounds.Height - tabPad.Top - tabPad.Bottom)
                };
                successful = (plotTab as mctcPlotGridViewTabControl).PopulateUI(mctcPlotDto, mctcBundle);
            }
            else
            {
                viewModel = new mctcPlotSingleViewModel() { Header = mctcPlotDto.Name, PresenterId = mctcPlotDto.Id };
                plotTab = new mctcPlotSingleViewTabControl()
                {
                    ViewModel = viewModel as mctcPlotSingleViewModel,
                    ParentWindow = this
                };
                successful = (plotTab as mctcPlotSingleViewTabControl).PopulateUI(mctcPlotDto, mctcBundle);
            }

            var plotType = mctcPlotDto.IsGridView ? "Grid-view" : "Single-view";
            if (successful)
            {
                viewModel.OwnerTabControl = plotTab;
                viewModel.CanBeRemoved = true;
                viewModel.TabColor = GraphicsGenerator.GetBrushFromColor(_viewModel.GetBundleColor(mctcBundle.Id));
                _viewModel.TabControls.Add(viewModel);
                _viewModel.mctcPlotDtos.Add(mctcPlotDto);
                tabControl.SelectedIndex = tabControl.Items.IndexOf(viewModel);

                var plotNode = new TreeNode(mctcPlotDto.Name) { PresenterId = mctcPlotDto.Id };
                plotNode.BorderColor = GraphicsGenerator.GetBrushFromColor(mctcBundle.Color);
                var mctcBundleNode = tvMctcBundles.Items.FirstOrDefault(x => (x as TreeNode).PresenterId == mctcBundle.Id) as TreeNode;
                if (mctcBundleNode != null)
                {
                    mctcBundleNode.Children.Add(plotNode);
                    mctcBundleNode.IsExpanded = true;
                    tvMctcBundles.SelectedItem = plotNode;
                }

                Log.Information("{type} plot added: {name}", plotType, mctcPlotDto.Name);
            }
            else
                Log.Error("Failed to add {type} plot: {name}", plotType, mctcPlotDto.Name);
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
                column.Width = new GridLength(220);

                splitter.MinWidth = 5;
                splitter.Width = new GridLength(5);
            }

            borderTreeLists.IsVisible = visible;
            splitterVertical.IsVisible = visible;
        }

        public void OpenExperiment(Guid experimenId)
        {
            _dataManagerTab?.OpenExperiment(experimenId);
            tabControl.SelectedItem = _dataManagerTab?.ViewModel;
        }

        public void OpenPhaseInExperiment(Guid experimentId, PhaseNameSet phaseNameSet, Guid? expRunId = null)
        {
            _dataManagerTab?.OpenPhaseInExperiment(experimentId, phaseNameSet, expRunId);
            tabControl.SelectedItem = _dataManagerTab?.ViewModel;
        }

        public void OpenExpRunInExperiment(Guid experimentId, Guid expRunId)
        {
            _dataManagerTab?.OpenExpRunInExperiment(experimentId, expRunId);
            tabControl.SelectedItem = _dataManagerTab?.ViewModel;
        }

        public void AddPhaseToCompare(Guid phaseId)
        {
            _dataManagerTab?.AddPhaseToCompare(phaseId);
        }
        #endregion
    }
}