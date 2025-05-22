using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.IO;
using System.Linq;
using Serilog;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.ViewModels;
using Xeovis.UI.ViewModels.DialogsViewModels;

namespace Xeovis.UI;

public partial class DataManagerLoadDataDialog : Window
{
    private DataManagerLoadDataViewModel _viewModel = new DataManagerLoadDataViewModel();

    public DataManagerLoadDataViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new DataManagerLoadDataViewModel();
            DataContext = _viewModel;
            PrepareUI();
        }
    }

    public DataManagerLoadDataDialog()
    {
        InitializeComponent();
        Log.Information("Dialog opened.");
        if (this.Content is Control content)
            content.DataContext = this.DataContext;
    }

    private void PrepareUI()
    {
        var vmDirectory = new LabelBrowseDirectoryControlViewModel("Directory", isFolderBrowser: true);
        vmDirectory.Directory = _viewModel.FolderDirectory;
        lbdcDirectory.ViewModel = vmDirectory;

        var vmCheckBox = new CheckBoxControlViewModel("Read Subfolders")
        {
            ToolTip = "Gives priority to the files the given subfolder contains.",
            IsChecked = _viewModel.IsSubfolderChecked
        };
        ckbxSubfolder.ViewModel = vmCheckBox;

        txtSubfolder.Text = _viewModel.SubfolderDirectory;

        var vmFolders = new LabelListControlViewModel();
        vmFolders.Label = "Folders";
        vmFolders.ToolTip = "Those that contain excel files.";
        vmFolders.IsCheckable = true;
        vmFolders.SetItems(_viewModel.EligibleFolders);
        vmFolders.SelectionMode = ItemSelectionMode.AlwaysSelected;
        foreach (var item in _viewModel.SelectedFolders)
            vmFolders.Items.First(i => i.Id == item.Id).IsChecked = true;

        llcFolders.ViewModel = vmFolders;
        llcFolders.ListSelectionChanged += llcFolders_ListSelectionChanged;
        llcFolders.ViewModel.ItemCheckStateChanged += ViewModel_ItemCheckStateChanged;

        var vmContents = new LabelListControlViewModel();
        vmContents.Label = "Folder Contents";
        llcContents.ViewModel = vmContents;

        var vmSelected = new LabelListControlViewModel();
        vmSelected.Label = "Selected Folders";
        vmSelected.IsRemovable = true;
        vmSelected.SetItems(_viewModel.SelectedFolders);
        llcSelected.ViewModel = vmSelected;
        llcSelected.ItemRemoved += llcSelected_ItemRemoved;
    }

    private void llcSelected_ItemRemoved(object? sender, Models.Events.ListableItemRemovedEventArgs e)
    {
        if (e.Item != null)
        {
            var item = llcFolders.ViewModel.Items.FirstOrDefault(i => i.Id == e.Item.Id);
            if (item == null) return;

            llcFolders.ViewModel.ItemCheckStateChanged -= ViewModel_ItemCheckStateChanged;
            item.IsChecked = false;
            llcFolders.ViewModel.ItemCheckStateChanged += ViewModel_ItemCheckStateChanged;
        }
    }

    private void ViewModel_ItemCheckStateChanged(object? sender, Models.Events.CheckStateChangedEventArgs e)
    {
        if (e.Tag is not ListableItem item) return;
        if (e.IsChecked)
        {
            if (!llcSelected.ViewModel.Items.Contains(item))
            {

                llcSelected.ViewModel.Items.Add(item);
                llcSelected.ViewModel.SetItems(llcSelected.ViewModel.Items.OrderBy(i => i.Label).ToList());
            }
        }
        else
        {
            llcSelected.ViewModel.Items.Remove(item);
        }
    }

    private void llcFolders_ListSelectionChanged(object? sender, Models.Events.ListSelectionChangedEventArgs? e)
    {
        var contents = _viewModel.GetFolderContents(e.SelectedItem as ListableItem);
        if (contents == null) return;
        llcContents.ViewModel.SetItems(contents);
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
        var folders = _viewModel.GetEligibleFoldersList(directory, ckbxSubfolder.ViewModel.IsChecked);
        
        if (folders == null || folders.Count == 0)
        {
            var msgBox = MessageBoxManager.GetMessageBoxStandard("Error", "No folder containing excel files was found!", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await msgBox.ShowWindowDialogAsync(this);
            return;
        }

        llcFolders.ViewModel.SetItems(folders.OrderBy(f => f.Label).ToList());
        llcSelected.ViewModel.Items.Clear();
    }

    private void btnCancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close(DialogResultType.Cancel);
    }

    private async void btnOK_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var selectedFolders = llcSelected.ViewModel.Items.ToList();
        if (selectedFolders == null || selectedFolders.Count == 0)
        {
            var msgBox = MessageBoxManager.GetMessageBoxStandard("Warning", "No folder was selected to read data.\nAre you sure you want to continue?\nThis might lead to losing previously loaded data!", ButtonEnum.YesNoCancel, MsBox.Avalonia.Enums.Icon.Warning);
            var result = await msgBox.ShowWindowDialogAsync(this);
            if (result != ButtonResult.Yes) return;
            Log.Information("User continued without any directory selected.");
        }

        _viewModel.SelectedFolders = selectedFolders;
        _viewModel.IsSubfolderChecked = ckbxSubfolder.IsChecked;
        _viewModel.SubfolderDirectory = txtSubfolder.Text;

        Log.Information("Experiment directories loaded.");
        this.Close(DialogResultType.OK);
    }
}