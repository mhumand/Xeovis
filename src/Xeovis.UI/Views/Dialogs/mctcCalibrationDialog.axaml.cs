using Avalonia.Controls;
using Avalonia.Interactivity;
using Serilog;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.ViewModels;
using Xeovis.UI.ViewModels.DialogsViewModels;

namespace Xeovis.UI.Views.Dialogs;

public partial class mctcCalibrationDialog : Window
{
    private mctcCalibrationViewModel _viewModel = new();
    
    public mctcCalibrationViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new ();
            DataContext = _viewModel;
            PrepareUI();
        }
    }
    
    public mctcCalibrationDialog()
    {
        InitializeComponent();
        Log.Information("Dialog opened.");
        if (this.Content is Control content)
            content.DataContext = this.DataContext;
    }

    private void PrepareUI()
    {
        if (_viewModel == null) return;
        
        var axfileDirectoryVM = new LabelBrowseDirectoryControlViewModel("ax-file Directory");
        axfileDirectoryVM.ExtensionType = FileExtensionType.Txt;
        lbdcAxFile.ViewModel = axfileDirectoryVM;
        
        var scriptFileDirectoryVM = new LabelBrowseDirectoryControlViewModel("Script file Directory");
        scriptFileDirectoryVM.ExtensionType = FileExtensionType.Txt;
        lbdcScriptFile.ViewModel = scriptFileDirectoryVM;

        var mctcVM = new LabelInputControlViewModel("MCTC Details", "", isMultiLine: true);
        txtMctc.ViewModel = mctcVM;

    }

    public void btnReadAxFile_OnClick(object? sender, RoutedEventArgs e)
    {
        var axfilePath = lbdcAxFile.GetDirectory();
        var xeosList = _viewModel.ReadAxFile(axfilePath);
        foreach (var xeos in xeosList)
        {
            
        }
    }


    private void btnCalculate_OnClick(object? sender, RoutedEventArgs e)
    {
        var fileName = lbdcScriptFile.GetDirectory();
        var mctcDetails = txtMctc.Value;
        _viewModel.Calculate(fileName,mctcDetails);
    }
}