using Avalonia.Controls;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Collections.Generic;
using Xeovis.UI.Models;
using Xeovis.UI.ViewModels;
using Xeovis.UI.ViewModels.DialogsViewModels;
using System.IO;
using System.Linq;
using Xeovis.UI.Models.Enums;

namespace Xeovis.UI;

public partial class mctcLoadDataDialog : Window
{
    private mctcLoadDataViewModel _viewModel = new mctcLoadDataViewModel();

    public mctcLoadDataViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new mctcLoadDataViewModel();
            DataContext = _viewModel;
            PrepareUI();
        }
    }

    private Dictionary<ListableItem, bool> _validationList;

    public mctcLoadDataDialog()
    {
        InitializeComponent();
        _validationList = new Dictionary<ListableItem, bool>();
    }

    private void PrepareUI()
    {
        if (_viewModel == null) return;

        var vmName = new LabelInputControlViewModel("Name", _viewModel.MctcBundleName);
        licName.ViewModel = vmName;

        var vmDirectory = new LabelBrowseDirectoryControlViewModel("Directory", isFolderBrowser: true);
        vmDirectory.Directory = _viewModel.FolderDirectory;
        lbdcDirectory.ViewModel = vmDirectory;

        var txtList = _viewModel.HasRequiredFiles ? _viewModel.RequiredFiles : _viewModel.GetRequiredFilesItems();
        var vmTxtFiles = new LabelListControlViewModel(txtList);
        vmTxtFiles.Label = "Required Files";
        vmTxtFiles.SelectedIndex = -1;
        llcTxtFiles.ViewModel = vmTxtFiles;
    }

    private async void btnScan_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var directory = lbdcDirectory.GetDirectory();
        if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
        {
            var msgBox = MessageBoxManager.GetMessageBoxStandard("Error", "Directory does not exist!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await msgBox.ShowWindowDialogAsync(this);
            return;
        }

        _viewModel.FolderDirectory = directory;
        _validationList = _viewModel.CheckRequiredFilesExistense();
        llcTxtFiles.ViewModel.SetItems(_validationList.Keys.ToList());
    }

    private void btnCancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close(DialogResultType.Cancel);
    }

    private async void btnLoad_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var items = llcTxtFiles.ViewModel.Items;
        if (_validationList == null || _validationList.Count == 0 || _validationList.Any(i => i.Value == false))
        {
            var msgBox = MessageBoxManager.GetMessageBoxStandard("Warning", "Could not find one or more required files." +
                "\nNo data will be loaded!" +
                "\nAre you sure you want to continue?", ButtonEnum.YesNoCancel, MsBox.Avalonia.Enums.Icon.Warning);
            var result = await msgBox.ShowWindowDialogAsync(this);
            if (result != ButtonResult.Yes)
                return;
        }

        _viewModel.MctcBundleName = licName.ViewModel.Value;

        this.Close(DialogResultType.OK);
    }
}