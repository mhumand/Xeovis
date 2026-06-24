using Avalonia.Controls;
using Avalonia.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Threading;
using Xeovis.Base;
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
using Xeovis.UI.Views.Dialogs;
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
        private CancellationTokenSource _activityCts;

        public MainWindow()
        {
            InitializeComponent();
            InitializeDefaultValues();
            Loaded += MainWindow_Loaded;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            PhaseNameList.PhaseNameListChanged += PhaseNameList_PhaseNameListChanged;
            xeosVariableNameList.xeosVariableNameListChanged += XeosVariableNameList_xeosVariableNameListChanged;
            PhaseComponentList.PhaseComponentListChanged += PhaseComponentListPhaseComponentListChanged;
            xeosVariableElementList.xeosVariableElementListChanged += XeosVariableElementListChanged;
            
            ApiHost.BundleReloadCalled += ApiHost_BundleReloadCalled;
            ApiHost.LoadNewBundleCalled += ApiHost_LoadNewBundleCalled;
            ApiHost.RefreshBundleUIAsked += ApiHost_RefreshBundleUIAsked;
            ApiHost.ApiActivityChanged += OnApiActivityChanged;
            PrepareUI();
        }

        private void OnApiActivityChanged(object? sender, ApiActivityChangedEventArgs e)
        {
            Dispatcher.UIThread.Post(async () =>
            {
                txtApiDot.Text = e.Message;
                stpApiDot.IsVisible = true;
                
                // Cancel any previous timer
                _activityCts?.Cancel();
                _activityCts = new CancellationTokenSource();

                try
                {
                    // Wait 1.5 seconds. If another request comes in, this gets cancelled.
                    await Task.Delay(2000, _activityCts.Token);
                
                    // If we make it here without being cancelled, traffic has stopped.
                    // IsApiActive = false;
                    stpApiDot.IsVisible = false;
                    txtApiDot.Text = string.Empty;
                }
                catch (TaskCanceledException)
                {
                    // Another request came in, keep blinking!
                }
            });
        }

        private void ApiHost_RefreshBundleUIAsked(object? sender, RefreshBundleUIAskedEventArgs e)
        {
            var bundle = _viewModel.mctcBundles.FirstOrDefault(x => x.Name.Equals(e.BundleName, StringComparison.OrdinalIgnoreCase));
            if (bundle == null) return;
            RepopulateBundleUI(bundle);
        }

        private void ApiHost_LoadNewBundleCalled(object? sender, LoadNewBundleCalledEventArgs e)
        {
            if (!Directory.Exists(e.Directory)) return;
            var bundleName = e.BundleName;
            //todo
        }

        private void ApiHost_BundleReloadCalled(object? sender, BundleReloadCalledEventArgs e)
        {
            var bundle = _viewModel.mctcBundles.FirstOrDefault(x => x.Name.Equals(e.BundleName, StringComparison.OrdinalIgnoreCase));
            if (bundle == null) return;
            if (e.IsReloadAndAppend)
                ReloadAndAppendBundle(bundle, simpleLoad: true);
            else
                ReloadBundle(bundle, simpleLoad: true);
        }

        private void InitializeDefaultValues()
        {
            DefaultPhaseNames.Initialize();
            PhaseNameList.Initialize();
            PhaseComponentList.Initialize();

            DefaultXeosVariableNames.Initialize();
            xeosVariableNameList.Initialize();
            xeosVariableElementList.Initialize();
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

        private void PhaseComponentListPhaseComponentListChanged(object? sender, PhaseComponentListChangedEventArgs e)
        {
            _viewModel.ProjectSettings.PhaseComponents = e.PhaseComponentList.ToDictionary(
                k => k.Key,
                v => v.Value.Select(x => x.Formula).ToList());
            RefreshAllTabControls();
        }

        private void XeosVariableElementListChanged(object? sender, xeosVariableElementListChangedEventArgs e)
        {
            _viewModel.ProjectSettings.xeosVariableElements = e.ElementList.ToDictionary(
                k => k.Key,
                v => v.Value.Select(x => x.Symbol).ToList());
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
            UIServices.ParentWindow = this;
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
            tvMctcBundles.DoubleTapped += tvMctcBundles_DoubleTapped;
            
            UIServices.ExperimentsLoaded += OnExperimentsLoaded;
            UIServices.mctcBundleLoaded += OnMctcBundleLoaded;
            UIServices.ProjectLoaded += OnProjectLoaded;

            _viewModel.ExperimentsDeleted += _viewModel_ExperimentsDeleted;
            _viewModel.mctcBundlesDeleted += _viewModel_mctcBundlesDeleted;
            _viewModel.mctcPlotsDeleted += _viewModel_mctcPlotsDeleted;

            tabControl.ContextRequested += TabControl_ContextRequested;
            tabControl.SelectionChanged += TabControl_SelectionChanged;
        }

        private void MovePlotTab(TabControlViewModelBase selectedTab, bool moveLeft)
        {
            if (selectedTab is DataManagerTabControlViewModel) return;
            var tabIndex = _viewModel.TabControls.IndexOf(selectedTab);
            var firstPlotIndex = _viewModel.TabControls.Any(x => x is DataManagerTabControlViewModel) ? 1 : 0;
            var plotIndex = tabIndex - firstPlotIndex;
            if (moveLeft)
            {
                if (tabIndex == firstPlotIndex) return;
                _viewModel.TabControls.Swap(tabIndex, tabIndex - 1);
                _viewModel.mctcPlotDtos.Swap(plotIndex, plotIndex - 1);
            }
            else // move right
            {
                if (tabIndex == _viewModel.TabControls.Count - 1) return;
                _viewModel.TabControls.Swap(tabIndex, tabIndex + 1);
                _viewModel.mctcPlotDtos.Swap(plotIndex, plotIndex + 1);
            }
            
            selectedTab.IsVisible = true;
            tabControl.SelectedItem = selectedTab;
            tabControl.Focus();
        }

        private void MovePlotTabToPosition(TabControlViewModelBase selectedTab, int positionInTabControl)
        {
            if (selectedTab is DataManagerTabControlViewModel) return;
            var tabIndex = _viewModel.TabControls.IndexOf(selectedTab);
            if (tabIndex == positionInTabControl) return; // no need to swap
            
            var firstPlotIndex = _viewModel.TabControls.Any(x => x is DataManagerTabControlViewModel) ? 1 : 0;
            if (positionInTabControl < firstPlotIndex) return;
            if (positionInTabControl > tabIndex) positionInTabControl--;
            
            _viewModel.TabControls.Move(tabIndex, positionInTabControl);
            _viewModel.mctcPlotDtos.Move(tabIndex - firstPlotIndex, positionInTabControl - firstPlotIndex);

            selectedTab.IsVisible = true;
            tabControl.SelectedItem = selectedTab;
            tabControl.Focus();
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
            if (e.Source is TextBlock txtblock && txtblock.Tag is TabControlViewModelBase selectedTab)
            {
                if (selectedTab is DataManagerTabControlViewModel)
                {
                    e.Handled = true;
                    return;
                }

                var plotDto = _viewModel.mctcPlotDtos.FirstOrDefault(x => x.Id == selectedTab.PresenterId);
                if (plotDto == null) return;
                
                var menu = new ContextMenu();

                // general items
                var renameItem = new MenuItem { Header = "Rename" };
                var otherNames = _viewModel.mctcPlotDtos.Except(new List<mctcPlotDto> { plotDto }).Select(x => x.Name).ToList();
                void Rename(mctcPlotDto plotDto, string newName)
                {
                    plotDto.Name = newName;
                    OnMctcPlotRenamed(plotDto);
                }
                renameItem.Click += (_, __) => RenameObjectWithDialog(plotDto, otherNames, x => x.Name, Rename);
                menu.Items.Add(renameItem);
                
                // moving tabs
                menu.Items.Add(new Separator());
                var firstPlotIndex = _viewModel.TabControls.Any(x => x is DataManagerTabControlViewModel) ? 1 : 0;
                var tabIndex = _viewModel.TabControls.IndexOf(selectedTab);
                var plotIndex = tabIndex - firstPlotIndex;

                if (plotIndex >= 1)
                {
                    var moveLeftItem = new MenuItem();
                    moveLeftItem.Header = "Move Left";
                    moveLeftItem.HotKey = new KeyGesture(Key.OemComma, KeyModifiers.Control);
                    moveLeftItem.InputGesture = new KeyGesture(Key.OemComma, KeyModifiers.Control);
                    moveLeftItem.Click += (_,_) => MovePlotTab(selectedTab, true);
                    menu.Items.Add(moveLeftItem);
                }

                if (tabIndex < _viewModel.TabControls.Count - 1)
                {
                    var moveRightItem = new MenuItem();
                    moveRightItem.Header = "Move Right";
                    moveRightItem.HotKey = new KeyGesture(Key.OemPeriod, KeyModifiers.Control);
                    moveRightItem.InputGesture = new KeyGesture(Key.OemPeriod, KeyModifiers.Control);
                    moveRightItem.Click += (_, _) => MovePlotTab(selectedTab, false);
                    menu.Items.Add(moveRightItem);
                }

                menu.Items.Add(new Separator());
                
                var moveBeforeItem = new MenuItem();
                moveBeforeItem.Header = "Move Before";
                var moveAfterItem = new MenuItem();
                moveAfterItem.Header = "Move After";
                for (var i = firstPlotIndex; i < _viewModel.TabControls.Count; i++)
                {
                    var targetTab = _viewModel.TabControls[i];
                    if (targetTab == selectedTab) continue; // don't show current tab

                    if (i != tabIndex + 1) // don't show the next tab for "Move Before"
                    {
                        var item = new MenuItem();
                        item.Header = targetTab.Header;
                        item.Tag = targetTab;
                        item.Click += (_, _) => MovePlotTabToPosition(selectedTab, _viewModel.TabControls.IndexOf(item.Tag as TabControlViewModelBase));
                        moveBeforeItem.Items.Add(item);
                    }

                    if (i != tabIndex - 1) // don't show the previous tab for "Move After"
                    {
                        var item = new MenuItem();
                        item.Header = targetTab.Header;
                        item.Tag = targetTab;
                        item.Click += (_, _) => MovePlotTabToPosition(selectedTab, _viewModel.TabControls.IndexOf(item.Tag as TabControlViewModelBase) + 1);
                        moveAfterItem.Items.Add(item);
                    }
                }
                menu.Items.Add(moveBeforeItem);
                menu.Items.Add(moveAfterItem);
                
                tabControl.ContextMenu = menu;
                tabControl.ContextMenu.Open();
            }
            else
            {
                e.Handled = true;
                tabControl.ContextMenu = null;
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
            UIServices.LoadProjectFile(path, raiseUnsavedChanges: true);
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

        private async Task OnProjectLoaded(object? sender, Models.Events.ProjectFileLoadedEventArgs e)
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
            
            if (settings.Elements?.Count > 0 && settings.Components.Count > 0)
            {
                Element.ClearAll();
                settings.Elements.ForEach(el => Element.GetOrRegister(el));
                
                Component.ClearAll();
                settings.Components.ToList().ForEach(p => Component.GetOrRegister(p.Key, p.Value));
            }
            
            PhaseNameList.SetPhaseNames(settings.AllowedPhaseNames, false);
            xeosVariableNameList.SetXeosVariableNames(settings.PhaseXeosVariables, false);

            var xeosVariableElements = settings.xeosVariableElements.ToDictionary(
                k => k.Key,
                v => v.Value.Select(el => Element.GetOrRegister(el)).ToList());
            xeosVariableElementList.SetXeosVariableElements(xeosVariableElements, false);
            
            var phaseComponents = settings.PhaseComponents.ToDictionary(
                k => k.Key,
                v => v.Value.Select(c => Component.Get(c)).ToList());
            PhaseComponentList.SetPhaseComponents(phaseComponents, false);
            
            var extension = Path.GetExtension(e.Path);
            this.Title = string.IsNullOrWhiteSpace(e.Project.ProjectName) ? "Xeovis" : $"Xeovis | {e.Project.ProjectName}{extension}";
            
            await Task.Delay(10);
            OnExperimentsLoaded(e.Project.Experiments);
            await Task.Delay(10);
            
            // e.Project.mctcBundles.ForEach(x => OnMctcBundleLoaded(x));
            // e.Project.mctcPlotDtos.ForEach(x => CreateMctcPlot(x));

            int counter = 0;
            foreach(var bundle in e.Project.mctcBundles)
            {
                OnMctcBundleLoaded(bundle);
                counter++;
                // If you have hundreds of bundles, yield every 10 items.
                // If you only have a few, just one Task.Delay after the loop is fine.
                if (counter % 10 == 0) await Task.Delay(1); 
            }
            
            await Task.Delay(10);

            foreach(var plot in e.Project.mctcPlotDtos)
            {
                CreateMctcPlot(plot);
                // Again, yield if you are generating many plots
                await Task.Delay(1); 
            }
            
            _viewModel.MctcBundleNodes.ToList().ForEach(x => x.IsExpanded = false);
            if (_viewModel.MctcBundleNodes.Count > 0)
                _viewModel.MctcBundleNodes.Last().IsExpanded = true;
        }

        private void OnMctcBundleLoaded(object? sender, Models.Events.mctcBundleLoadedEventArgs e)
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

        private void OnExperimentsLoaded(object? sender, Models.Events.ExperimentsLoadedEventArgs e)
        {
            OnExperimentsLoaded(e.Experiments);
        }

        private void OnExperimentsLoaded(List<Experiment> experiments)
        {
            _viewModel.ExperimentNodes.Clear();
            _viewModel.Experiments = experiments;
            foreach (var exp in experiments)
            {
                var expNode = new TreeNode(exp.Name)
                    { 
                        PresenterId = exp.Id,
                        Description = exp.ShortName,
                        DescriptionColor = System.Drawing.Color.Gray
                    };
                
                foreach (var phase in exp.PhasesWithData)
                {
                    var phaseNode = new TreeNode(phase.ToString())
                    {
                        PresenterId = exp.Id,
                        Description = phase.KeyName,
                        DescriptionColor = System.Drawing.Color.DarkGray,
                        Tag = phase
                    };
                    
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
            if (_viewModel.mctcBundles.Count == 0 || sender is not TreeView { SelectedItem: TreeNode node })
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

            var settingsItem = new MenuItem() { Header = "Settings" };
            
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
                addPlotItem.Click += (s, e) => UIServices.LoadNewPlot(node.PresenterId);
                menu.Items.Insert(0, addPlotItem);

                var reloadItem = new MenuItem() { Header = "Reload" };
                ToolTip.SetTip(reloadItem, $"Reloads the bundle's contents from its directory:\n{bundle.FilesDirectory}");
                reloadItem.Click += reloadItem_Click;
                reloadItem.Tag = bundle;
                menu.Items.Insert(1, reloadItem);

                var reloadAndAppendItem = new MenuItem() { Header = "Reload & Append" };
                ToolTip.SetTip(reloadAndAppendItem, "Reloads the bundle's contents from its directory and appends them to the existing content.");
                reloadAndAppendItem.Click += reloadAndAppendItem_Click; ;
                reloadAndAppendItem.Tag = bundle;
                menu.Items.Insert(2, reloadAndAppendItem);

                deleteItem.Click += (s, e) => RequestRemovalConfirmationAndProceed(() => _viewModel.DeleteMctcBundle(node.PresenterId), $"{node.Label} and its plots");
                deleteAllItem.Click += (s, e) => RequestRemovalConfirmationAndProceed(() => _viewModel.DeleteAllMctcBundles(), "all mctc bundles and their plots");
                deleteAllItem.Header = "Delete All Bundles";
                
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

                menu.Items.Add(new Separator());
                
                // Go to Directory
                if (DirectoryHelper.DirectoryExists(bundle.FilesDirectory))
                {
                    var directoryItem = new MenuItem() { Header = "Go to Directory" };
                    directoryItem.Click += (s, e) => DirectoryHelper.GoToDirectory(bundle.FilesDirectory);
                    menu.Items.Add(directoryItem);
                }
                
                // Settings & Info
                settingsItem.Header = "Settings & Info";
                settingsItem.Click += async (s, e) =>
                {
                    var bundleInfoVM = new mctcBundleInfoViewModel(bundle, otherNames);
                    var bundleInfoDialog = new mctcBundleInfoDialog() { ViewModel = bundleInfoVM };
                    var result = await bundleInfoDialog.ShowDialog<DialogResultType>(this);
                    if (result is DialogResultType.OK)
                    {
                        ChangeBundleColor(bundle, bundleInfoDialog.Color);
                        bundle.FilesDirectory = bundleInfoDialog.Directory;
                        Rename(bundle, bundleInfoDialog.Name);
                    }
                };
                menu.Items.Add(settingsItem);
            }
            else // mctc plot
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
                    node.HasVisiblePlot = true;
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
                    node.HasVisiblePlot = visibleItem.IsChecked;
                };
                menu.Items.Add(visibleItem);

                deleteItem.Click += (s, e) => RequestRemovalConfirmationAndProceed(() => _viewModel.DeleteMctcPlot(node.PresenterId), $"{node.Label} in {bundle.Name}");
                deleteAllItem.Click += (s, e) => RequestRemovalConfirmationAndProceed(() => _viewModel.DeleteAllMctcPlotsInBundle(node.PresenterId), $"all plots in {bundle.Name}");
                deleteAllItem.Header = "Delete All Plots";
                
                // rename
                var otherNames = _viewModel.mctcPlotDtos.Except(new List<mctcPlotDto> { mctcPlotDto }).Select(x => x.Name).ToList();
                void Rename(mctcPlotDto plotDto, string newName)
                {
                    plotDto.Name = newName;
                    OnMctcPlotRenamed(plotDto);
                }
                renameItem.Click += (s, e) => RenameObjectWithDialog(mctcPlotDto, otherNames, x => x.Name, Rename);
                
                menu.Items.Add(new Separator());
                
                // export
                if (tab.OwnerTabControl is mctcPlotGridViewTabControl gridTab)
                {
                    var exportItem = new MenuItem() { Header = "Export" };
                    exportItem.Click += async (sender, e) =>
                    {
                        await UIServices.OpenMctcGridExportDialog(mctcPlotDto, gridTab.uniformGrid);

                    };
                    menu.Items.Add(exportItem);
                }
                
                // change variables
                var changeVarsItem = new MenuItem() { Header = "Change Variables" };
                changeVarsItem.Click += async (s, e) =>
                {
                    if (tab.OwnerTabControl is ImctcPlotView mctcPlot)
                        await UIServices.OpenMctcPlotDialog(bundle, mctcPlotDto, mctcPlot);
                };
                menu.Items.Add(changeVarsItem);
                
                // settings
                settingsItem.Click += (s, e) => ShowMctcPlotSettingsDialog(node.PresenterId);
                menu.Items.Add(settingsItem);
            }

            tvMctcBundles.ContextMenu = menu;
            tvMctcBundles.ContextMenu.Open();
        }

        private void ChangeBundleColor(mctcBundle bundle, Color newColor)
        {
            var result = UIServices.ChangeBundleColor(bundle, newColor);
            if (!result) return;
            _viewModel.mctcBundleColors[bundle.Id] = bundle.Color;
            
            var bundleNode = tvMctcBundles.Items.FirstOrDefault(x => x is TreeNode node && node.PresenterId == bundle.Id) as TreeNode;
            if (bundleNode == null) return;
            bundleNode.BorderColor = GraphicsGenerator.GetBrushFromColor(bundle.Color);
            foreach (TreeNode child in bundleNode.Children)
            {
                child.BorderColor = GraphicsGenerator.GetBrushFromColor(bundle.Color);
            }
            
            var plotsID = _viewModel.mctcPlotDtos.Where(x => x.mctcBundleId == bundle.Id).Select(x => x.Id).ToList();
            var tabs = _viewModel.TabControls.Where(x => plotsID.Contains(x.PresenterId));
            foreach (var tab in tabs)
            {
                tab.TabColor = GraphicsGenerator.GetBrushFromColor(bundle.Color);
            }
        }
        
        private void tvMctcBundles_DoubleTapped(object? sender, TappedEventArgs e)
        {
            if (tvMctcBundles.SelectedItem is not TreeNode node) return;

            var isMctcBundle = _viewModel.mctcBundles.Any(x => x.Id == node.PresenterId);
            if (isMctcBundle) return;

            var mctcPlotDto = _viewModel.mctcPlotDtos.FirstOrDefault(x => x.Id == node.PresenterId);
            var tab = tabControl.Items.FirstOrDefault(x => (x as TabControlViewModelBase)?.PresenterId == node.PresenterId) as TabControlViewModelBase;

            if (mctcPlotDto == null || tab == null) return;

            tab.IsVisible = true;
            mctcPlotDto.IsVisible = true;
            node.HasVisiblePlot = true;
            tabControl.SelectedItem = tab;
            
            e.Handled = true;
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
                openItem.Click += (s, e) => OpenExperiment(node.PresenterId);

                var changeShortNameItem = new MenuItem() { Header = "Change Short Name" };
                void Rename(Experiment exp, string newName)
                {
                    exp.ShortName = newName;
                    OnExperimentShortNameChanged(exp);
                }
                changeShortNameItem.Click += (s, e) =>
                {
                    var otherNames = _viewModel.Experiments.Except([experiment]).Select(x => x.ShortName).ToList();
                    RenameObjectWithDialog(experiment, otherNames, x => x.ShortName, Rename, maxNameLength: 10);
                };
                
                menu.Items.Add(changeShortNameItem);
                
                menu.Items.Add(new Separator());

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
            await ReloadBundle(bundle);
        }

        private async Task ReloadBundle(mctcBundle bundle, bool simpleLoad = false)
        {
            var newBundle = simpleLoad ? UIServices.SimpleMctcBundleLoad(bundle.FilesDirectory) :
                await UIServices.LoadMctcBundle(bundle.FilesDirectory);
            if (newBundle != null)
            {
                newBundle.Id = bundle.Id;
                newBundle.Name = bundle.Name;
                newBundle.Color = bundle.Color;
                var index = _viewModel.mctcBundles.FindIndex(x => x.Id == newBundle.Id);
                _viewModel.mctcBundles[index] = newBundle;

                if (!simpleLoad) RepopulateBundleUI(newBundle);
                Log.Information("Bundle reloaded: {name}", bundle.Name);
            }
            else
            {
                Log.Error("Failed to reload mctc bundle: {name}", bundle.Name);
                var mb = MessageBoxManager.GetMessageBoxStandard("Error", $"Failed to reload {bundle.Name}!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await mb.ShowWindowDialogAsync(this);
            }
        }

        private async void RepopulateBundleUI(mctcBundle mctcBundle)
        {
            if (mctcBundle == null) return;
            foreach (var plotDto in _viewModel.mctcPlotDtos.Where(x => x.mctcBundleId == mctcBundle.Id))
            {
                var tab = _viewModel.TabControls.FirstOrDefault(x => x.PresenterId == plotDto.Id);
                // if (tab?.OwnerTabControl is ImctcPlotView mctcPlot)
                // {
                //     // 1. Use InvokeAsync and await it. This ensures we don't start populating 
                //     // the NEXT tab until this current one is completely finished.
                //     await Dispatcher.UIThread.InvokeAsync(() => mctcPlot.PopulateUI(plotDto, mctcBundle));
                //
                //     // 2. THE YIELD: Pause for 10 milliseconds before looping. 
                //     // This tiny gap allows Avalonia's Render Engine to step in and update the 
                //     // blinking green dot on the screen so it stays perfectly smooth!
                //     await Task.Delay(10);
                // }
                
                if (tab?.OwnerTabControl is ImctcPlotView mctcPlot)
                    Dispatcher.UIThread.Post(() => mctcPlot.PopulateUI(plotDto, mctcBundle));
            }
        }
        
        private async void reloadAndAppendItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem) return;
            if (menuItem.Tag is not mctcBundle bundle) return;
            await ReloadAndAppendBundle(bundle);
        }

        private async Task ReloadAndAppendBundle(mctcBundle bundle, bool simpleLoad = false)
        {
            var newBundle = simpleLoad ? UIServices.SimpleMctcBundleLoad(bundle.FilesDirectory) :
                await UIServices.LoadMctcBundle(bundle.FilesDirectory);
            if (newBundle != null)
            {
                // 1. Create an O(1) Dictionary by generating a unique string key for each batch
                var oldBatchesDict = bundle.Batches.ToDictionary(
                    b => b.Name + "|" + string.Join(",", b.PhaseNameIds.OrderBy(id => id))
                );
                
                foreach (var newBatch in newBundle.Batches)
                {
                    // 2. Generate the key for the incoming batch
                    var key = newBatch.Name + "|" + string.Join(",", newBatch.PhaseNameIds.OrderBy(id => id));
                    
                    // var oldBatch = bundle.Batches.FirstOrDefault(x => x.Name.Equals(newBatch.Name)
                    // && newBatch.PhaseNameIds.SequenceEqual(x.PhaseNameIds));
                    // if (oldBatch == null)
                    //     bundle.Batches.Add(newBatch);
                    
                    // 3. Instant O(1) lookup
                    if (!oldBatchesDict.TryGetValue(key, out var oldBatch))
                    {
                        bundle.Batches.Add(newBatch);
                    }
                    else 
                    {
                        // overwrites the xeos var in the old batch
                        var vars = newBundle.xeosVariables.Where(x => x.BatchId == newBatch.Id).ToList();
                        vars.ForEach(x => x.BatchId = oldBatch.Id);
                        oldBatch.xeosVariables = vars;
                    }
                }

                if (!simpleLoad) RepopulateBundleUI(bundle);
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

        public async void RenameObjectWithDialog<T>(T target, List<string> existingNames, 
            Func<T, string> getName, Action<T, string> setName, int maxNameLength = 100)
        {
            string currentName = getName(target);

            var dialog = new RenameDialog(currentName, existingNames, maxNameLength);
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

        public void OnExperimentShortNameChanged(Experiment experiment)
        {
            var expNode = _viewModel.ExperimentNodes.FirstOrDefault(x => x.PresenterId == experiment.Id);
            if (expNode == null) return;
            var oldName = expNode.Description;
            expNode.Description = experiment.ShortName;
            _dataManagerTab?.UpdateExperimentShortName(experiment.Id, experiment.ShortName);
            Log.Information("Experiment short name changed: {old} -> {new}", oldName, experiment.ShortName);
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
            var bundle = _viewModel.mctcBundles.FirstOrDefault(x => x.Id == mctcPlotDto.mctcBundleId);
            var tabVM = _viewModel.TabControls.FirstOrDefault(x => x.PresenterId == Id);
            var tab = tabVM?.OwnerTabControl as ImctcPlotView;
            if (bundle == null || tab == null) return;
            await UIServices.OpenMctcPlotSettings(tab, mctcPlotDto, bundle);
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
                _viewModel.mctcPlotDtos.Remove(plot);
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
                };
                successful = (plotTab as mctcPlotSingleViewTabControl).PopulateUI(mctcPlotDto, mctcBundle);
            }

            var plotType = mctcPlotDto.IsGridView ? "Grid-view" : "Single-view";
            if (successful)
            {
                viewModel.OwnerTabControl = plotTab;
                viewModel.CanBeRemoved = true;
                viewModel.TabColor = GraphicsGenerator.GetBrushFromColor(mctcBundle.Color);
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