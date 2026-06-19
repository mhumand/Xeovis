using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Serilog;
using Xeovis.Base.Validations;
using Xeovis.Domain;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.Models.Events;
using Xeovis.UI.ViewModels;
using Xeovis.UI.ViewModels.DialogsViewModels;

namespace Xeovis.UI.Views.Dialogs;

public partial class mctcBundleInfoDialog : Window
{
    private mctcBundleInfoViewModel _viewModel = new();
    
    private mctcBundle _bundle => _viewModel.mctcBundle;
    
    public string Directory { get; set; }
    public string Name { get; set; }
    public Color Color { get; set; }
    
    public mctcBundleInfoViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new ();
            DataContext = _viewModel;
            PrepareUI();
        }
    }
    
    public mctcBundleInfoDialog()
    {
        InitializeComponent();
        Log.Information("Dialog opened.");
        if (this.Content is Control content)
            content.DataContext = this.DataContext;
        
        btnCancel.Click += btnCancel_Click;
        btnSave.Click += btnSave_Click;
        btnRestoreDirectory.Click += btnRestoreDirectoryOnClick;
        ckbxSingleExpansion.CheckStateChanged += ckbxSingleExpansionOnCheckStateChanged;
        cmbxBatch.ListSelectionChanged += cmbxBatchOnListSelectionChanged;
    }
    
    private void PrepareUI()
    {
        if (_bundle == null) return;

        licName.ViewModel = new LabelInputControlViewModel("Name", _bundle.Name)
        {
            ValidationRules = new List<IValidationRule<string>>()
            {
                new NotEmptyOrWhitespaceRule(),
                new DuplicateNameRule(_viewModel.OtherNames)
            }
        };
        
        lbdcDirectory.ViewModel = new LabelBrowseDirectoryControlViewModel("Directory", _bundle.FilesDirectory, isFolderBrowser: true);;
        ckbxSingleExpansion.ViewModel = new CheckBoxControlViewModel("Single Expansion", true);
        SetupColorPicker();
        

        cmbxBatch.ViewModel = new LabelComboBoxControlViewModel() {Label = "Select Batch", IsStackedHorizontally = true};
        cmbxBatch.SetItems(_viewModel.GetBatcheListItems());
        
        tableBatches.PopulateTable(_viewModel.GetBatchesTableModel());
        tableMctcVariables.PopulateTable(_viewModel.GetMctcVariablesTableModel());
        
        expanderBatches.Header = $"Batches [{_bundle.Batches.Count}]";
        expanderXeosVariables.Header = $"x-eos Variables [{_bundle.xeosVariables.Count}]";
        expanderMctcVariables.Header = $"mctc Variables [{_bundle.mctcVariables.Count}]";
    }

    private void SetupColorPicker()
    {
        colorPicker.Color = GraphicsGenerator.GetAvaloniaColor(_bundle.Color);
        colorPicker.Palette = new MaterialHalfColorPalette();
        colorPicker.IsAlphaVisible = false;
        colorPicker.IsHexInputVisible = false;
        // colorPicker.ColorChanged += (s, e) => ChangeBundleColor(bundle, e.NewColor);
    }
    
    private void cmbxBatchOnListSelectionChanged(object? sender, ListSelectionChangedEventArgs e)
    {
        var table = _viewModel.GetXeosVariablesTableModel(cmbxBatch.ViewModel.SelectedItem.PresenterId);
        tableXeosVariables.PopulateTable(table);
    }
    
    private void btnRestoreDirectoryOnClick(object? sender, RoutedEventArgs e)
    {
        lbdcDirectory.ViewModel.Directory = _bundle.FilesDirectory;
    }
    
    private void ckbxSingleExpansionOnCheckStateChanged(object? sender, CheckStateChangedEventArgs e)
    {
        var expanders = ExpandersContainer.Children.Cast<Expander>();
        if (!e.IsChecked || expanders.Count(x => x.IsExpanded) <= 1) return;
        foreach (var child in expanders)
            child.IsExpanded = false;
        
        expanders.First().IsExpanded = true;
    }

    private void Expander_Expanded(object? sender, RoutedEventArgs e)
    {
        if (!ckbxSingleExpansion.IsChecked) return;
        if (sender is Expander currentlyOpenedExpander)
        {
            foreach (var child in ExpandersContainer.Children)
            {
                if (child is Expander otherExpander && otherExpander != currentlyOpenedExpander)
                {
                    otherExpander.IsExpanded = false;
                }
            }
        }
    }
    
    private void btnSave_Click(object? sender, RoutedEventArgs e)
    {
        Directory = lbdcDirectory.ViewModel.Directory;
        Name = licName.ViewModel.Value;
        Color = colorPicker.Color;
        this.Close(DialogResultType.OK);
    }

    private void btnCancel_Click(object? sender, RoutedEventArgs e)
    {
        this.Close(DialogResultType.Cancel);
    }

}