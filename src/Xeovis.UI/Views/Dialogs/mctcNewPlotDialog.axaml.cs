using Avalonia.Controls;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Domain;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.Models.Events;
using Xeovis.UI.ViewModels;
using Xeovis.UI.ViewModels.DialogsViewModels;

namespace Xeovis.UI;

public partial class mctcNewPlotDialog : Window
{
    private mctcNewPlotViewModel _viewModel = new mctcNewPlotViewModel();

    public mctcNewPlotViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new mctcNewPlotViewModel();
            DataContext = value;
            PrepareUI();
        }
    }

    public mctcPlotDto mctcPlotDto { get; set; }

    private List<mctcBundle> _bundles => _viewModel.mctcBundles;

    public mctcNewPlotDialog()
    {
        InitializeComponent();
        mctcPlotDto = new mctcPlotDto();
        llcSelected.ItemRemoved += llcSelected_ItemRemoved;
        lcbcMctcBundles.ListSelectionChanged += lcbcMctcBundles_ListSelectionChanged;
        llcPhase.ListSelectionChanged += llcPhase_ListSelectionChanged;
    }

    public mctcNewPlotDialog(List<mctcBundle> mctcBundles) : this()
    {
        _viewModel.mctcBundles = mctcBundles;
        PrepareUI();
    }

    public mctcNewPlotDialog(List<mctcBundle> mctcBundles, mctcPlotDto mctcPlotDto) : this()
    {
        this.mctcPlotDto = mctcPlotDto;
        _viewModel.mctcBundles = mctcBundles;
        _viewModel.SelectedBundleId = mctcPlotDto.mctcBundleId;
        _viewModel.SelectedXeosVariables = mctcPlotDto.SampleXeosVariables;
        _viewModel.MakePhaseVaribleItemsDictionary(mctcBundles.FirstOrDefault(b => b.Id == mctcPlotDto.mctcBundleId));
        PrepareUI();
    }

    public void PrepareUI()
    {
        var vmName = new LabelInputControlViewModel("Name", mctcPlotDto.Name);
        txtName.ViewModel = vmName;

        var vmPhase = new LabelListControlViewModel();
        vmPhase.Label = "Phase";
        vmPhase.SelectionMode = ItemSelectionMode.AlwaysSelected;
        llcPhase.ViewModel = vmPhase;

        var vmXeosVariables = new LabelListControlViewModel();
        vmXeosVariables.Label = "x-eos Variables";
        vmXeosVariables.IsCheckable = true;
        llcXeosVariables.ViewModel = vmXeosVariables;
        llcXeosVariables.ViewModel.ItemCheckStateChanged += llcXeosVariables_ItemCheckStateChanged;

        var vmSelected = new LabelListControlViewModel();
        vmSelected.Label = "Selected Variables";
        vmSelected.IsRemovable = true;
        llcSelected.ViewModel = vmSelected;

        var vmTileView = new CheckBoxControlViewModel("Grid-view for multiple plots");
        vmTileView.ToolTip = "If checked, the selected variables will be plotted together in a grid.";
        ckbxTileView.ViewModel = vmTileView;

        var vmMctcBundle = new LabelComboBoxControlViewModel();
        vmMctcBundle.Label = "mctc Bundle";
        foreach (var bundle in _bundles)
            vmMctcBundle.Items.Add(new ListableItem(bundle.Name, bundle.Id));

        var selectedItem = vmMctcBundle.Items.FirstOrDefault(i => i.PresenterId == _viewModel.SelectedBundleId);
        vmMctcBundle.SelectedItem = selectedItem ?? vmMctcBundle.Items[0];
        lcbcMctcBundles.ViewModel = vmMctcBundle;
    }

    private void lcbcMctcBundles_ListSelectionChanged(object? sender, ListSelectionChangedEventArgs e)
    {
        if (e.SelectedItem is not ListableItem item) return;
        var bundle = _bundles.FirstOrDefault(i => i.Id == item.PresenterId);
        if (bundle == null) return;
        _viewModel.SelectedXeosVariables.Clear();
        _viewModel.SelectedBundleId = bundle.Id;
        var phaseItems = _viewModel.MakePhaseVaribleItemsDictionary(bundle).Keys.ToList();
        llcPhase.ViewModel.SetItems(phaseItems);
    }

    private void llcSelected_ItemRemoved(object? sender, ListableItemRemovedEventArgs e)
    {
        if (e.Item != null && e.Item.Tag is xeosVariable variable)
        {
            _viewModel.SelectedXeosVariables.Remove(variable);
            llcXeosVariables.ViewModel.ItemCheckStateChanged -= llcXeosVariables_ItemCheckStateChanged;
            e.Item.IsChecked = false;
            llcXeosVariables.ViewModel.ItemCheckStateChanged += llcXeosVariables_ItemCheckStateChanged;
        }
    }

    private void llcXeosVariables_ItemCheckStateChanged(object? sender, CheckStateChangedEventArgs e)
    {
        if (e.Tag is not ListableItem item) return;
        if (item.Tag is not xeosVariable variable) return;
        if (e.IsChecked)
        {
            if (!llcSelected.ViewModel.Items.Contains(item))
            {
                llcSelected.ViewModel.Items.Add(item);
                _viewModel.SelectedXeosVariables.Add(variable);
            }
        }
        else
        {
            llcSelected.ViewModel.Items.Remove(item);
            _viewModel.SelectedXeosVariables.Remove(variable);
        }
    }

    private void llcPhase_ListSelectionChanged(object? sender, ListSelectionChangedEventArgs e)
    {
        if (e.SelectedItem is not ListableItem item) return;
        if (item.Tag is not PhaseType phaseType) return;
        var bundle = _bundles.FirstOrDefault(i => i.Id == lcbcMctcBundles.ViewModel.SelectedItem.PresenterId);
        if (bundle == null) return;
        var xeosVariableItems = _viewModel.AllPhaseVariableItems.FirstOrDefault(i => (PhaseType)i.Key.Tag == phaseType).Value;
        llcXeosVariables.ViewModel.SetItems(xeosVariableItems);
        FetchXeosVariablesCheckStatus();
    }

    private void FetchXeosVariablesCheckStatus()
    {
        foreach (var item in llcXeosVariables.ViewModel.Items)
        {
            if (item.Tag is not xeosVariable xeosVar) return;
            if (_viewModel.SelectedXeosVariables.Any(v => v.PhaseType == xeosVar.PhaseType && v.VariableType == xeosVar.VariableType))
                item.IsChecked = true;
            else
                item.IsChecked = false;
        }
    }

    private async void btnPlot_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_viewModel.SelectedXeosVariables.Count == 0)
        {
            var msgBox = MessageBoxManager.GetMessageBoxStandard("Warning", "No x-eos variable was selected.\nAre you sure you want to continue?", ButtonEnum.YesNoCancel, MsBox.Avalonia.Enums.Icon.Warning);
            var result = await msgBox.ShowWindowDialogAsync(this);
            if (result != ButtonResult.Yes)
            {
                mctcPlotDto = null;
                return;
            }
        }

        mctcPlotDto = new mctcPlotDto(_viewModel.SelectedXeosVariables, _viewModel.SelectedBundleId)
        {
            Name = txtName.ViewModel.Value,
            IsGridView = ckbxTileView.IsChecked
        };

        this.Close(DialogResultType.OK);
    }

    private void btnCancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close(DialogResultType.Cancel);
    }
}