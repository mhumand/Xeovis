using Avalonia.Controls;
using Xeovis.UI.ViewModels;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Events;
using System.Drawing;
using System.Collections.Generic;

namespace Xeovis.TestUI;

public partial class FrmLabelListControl : Window
{
    public FrmLabelListControl()
    {
        InitializeComponent();
        DataContext = this;

        PrepareUI();
        llcList.ListSelectionChanged += llcList_ListSelectionChanged;
    }

    private void PrepareUI()
    {
        var items = new List<ListableItem>()
        {
            new ListableItem("Item 1"),
            new ListableItem("Item 2"),
            new ListableItem("Item 3",isVisible:false),
            new ListableItem("Item 4"),
            new ListableItem("Item 5"),
            new ListableItem("Item 6"),
            new ListableItem("Item 7"),
            new ListableItem("Item 8"),
            new ListableItem("Item 9"),
            new ListableItem("Item 10"),
        };

        var vm = new LabelListControlViewModel(items);

        for (int i = 0; i < vm.Items.Count; i++)
        {
            vm.Items[i].Tag = i;
        }

        vm.Items[0].Description = "item 1";
        vm.Items[1].Description = "item 2";
        vm.Items[1].Color = Color.DarkRed;
        vm.Items[5].IsChecked = true;
        vm.Items[7].IsChecked = true;

        vm.Label = "Simple Label";
        vm.ToolTip = "Simple tooltip for label";
        vm.SelectedIndex = 6;
        vm.IsCheckable = true;
        vm.IsRemovable = true;
        vm.ItemCheckStateChanged += Vm_ItemCheckStateChanged;
        llcList.ViewModel = vm;
    }

    private void Vm_ItemCheckStateChanged(object? sender, CheckStateChangedEventArgs e)
    {
        if (e.IsChecked != null)
            tbCheckedItem.Text = "Check status changed for item " + sender.ToString() + " : " + e.IsChecked.ToString();
        else
            tbCheckedItem.Text = "";
    }

    private void llcList_ListSelectionChanged(object? sender, ListSelectionChangedEventArgs e)
    {
        if (e.SelectedItem != null)
            tbSelectedItem.Text = "Selected Item = " + e.SelectedItem.ToString();
        else
            tbSelectedItem.Text = "";
    }

    private void btnAdd_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        llcList.ViewModel.Items.Add(new ListableItem("Added Item"));
    }
}