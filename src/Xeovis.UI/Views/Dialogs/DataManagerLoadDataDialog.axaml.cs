using Avalonia.Controls;
using System.Linq;
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
            _viewModel = value;
            DataContext = value;
            PrepareUI();
        }
    }

    public DataManagerLoadDataDialog()
    {
        InitializeComponent();
        PrepareUI();
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
        llcSelected.ItemRemoved += LlcSelected_ItemRemoved;
    }

    private void LlcSelected_ItemRemoved(object? sender, Models.Events.ListableItemRemovedEventArgs e)
    {
        if (e.Item != null)
        {
            var item = llcFolders.ViewModel.Items.FirstOrDefault(i => i.Id == e.Item.Id);
            if (item == null) return;
            var newItem = item.Copy();
            newItem.IsChecked = false;
            var index = llcFolders.ViewModel.Items.IndexOf(item);
            llcFolders.ViewModel.Items.RemoveAt(index);
            llcFolders.ViewModel.Items.Insert(index, newItem);
        }
    }

    private void ViewModel_ItemCheckStateChanged(object? sender, Models.Events.CheckStateChangedEventArgs e)
    {
        if (e.Tag is not ListableItem item) return;
        if (e.IsChecked)
        {
            if (!llcSelected.ViewModel.Items.Contains(item))
                llcSelected.ViewModel.Items.Add(item);
        }
        else
        {
            llcSelected.ViewModel.Items.Remove(item);
        }
    }

    private void llcFolders_ListSelectionChanged(object? sender, Models.Events.ListSelectionChangedEventArgs e)
    {
        var contents = _viewModel.GetFolderContents(e.SelectedItem as ListableItem);
        if (contents == null) return;
        llcContents.ViewModel.SetItems(contents);
    }

    private void btnScan_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _viewModel.FolderDirectory = lbdcDirectory.ViewModel.Directory;
        var folders = _viewModel.GetEligibleFoldersList(_viewModel.FolderDirectory, ckbxSubfolder.ViewModel.IsChecked);
        if (folders == null) return;
        llcFolders.ViewModel.SetItems(folders);
        llcSelected.ViewModel.Items.Clear();
    }

    private void btnCancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close(DialogResultType.Cancel);
    }

    private void btnOK_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        this.Close(DialogResultType.OK);
    }
}