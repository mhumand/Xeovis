using Avalonia.Controls;
using Xeovis.UI.ViewModels;
using Xeovis.UI.Models;
using System.Collections.ObjectModel;
using Xeovis.UI.Events;
using System.Drawing;

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
        var vm = new LabelListControlViewModel();
        vm.Items = new ObservableCollection<ListableItem>()
        {
            new ListableItem("Item 1"),
            new ListableItem("Item 2"),
            new ListableItem("Item 3",isVisible:false),
            new ListableItem("Item 4")
        };

        for (int i = 0; i < vm.Items.Count; i++)
        {
            vm.Items[i].SetTag(i);
        }

        vm.Items[0].CanBeRemoved = true;
        vm.Items[0].SetDescription("item 1");
        vm.Items[1].SetDescription("item 2");
        vm.Items[1].SetColor(Color.DarkRed);

        vm.Label = "Simple Label";
        llcList.SetDataContext(vm);
    }

    private void llcList_ListSelectionChanged(object? sender, ListSelectionChangedEventArgs e)
    {
        if (e.SelectedItem != null)
            tbSelectedItem.Text = e.SelectedItem.ToString();
        else
            tbSelectedItem.Text = "";
    }
}