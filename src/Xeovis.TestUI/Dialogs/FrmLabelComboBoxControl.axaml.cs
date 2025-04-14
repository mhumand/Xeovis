using Avalonia.Controls;
using System.Collections.ObjectModel;
using Xeovis.UI.ViewModels;

namespace Xeovis.TestUI;

public partial class FrmLabelComboBoxControl : Window
{
    public FrmLabelComboBoxControl()
    {
        InitializeComponent();
        DataContext = this;

        PrepareUI();
    }

    private void PrepareUI()
    {
        var vm1 = new LabelComboBoxControlViewModel();
        vm1.Label = "ComboBox 1";
        vm1.Items = new ObservableCollection<UI.Models.ListableItem>()
        {
            new UI.Models.ListableItem("item 1"),
            new UI.Models.ListableItem("item 2"),
            new UI.Models.ListableItem("item 3"),
            new UI.Models.ListableItem("item 4"),
            new UI.Models.ListableItem("item 5"),
            new UI.Models.ListableItem("item 6"),
            new UI.Models.ListableItem("item 7"),
            new UI.Models.ListableItem("item 8"),
            new UI.Models.ListableItem("item 9"),
            new UI.Models.ListableItem("item 10"),
            new UI.Models.ListableItem("item 11"),
            new UI.Models.ListableItem("item 12"),
            new UI.Models.ListableItem("item 13"),
            new UI.Models.ListableItem("item 14"),
            new UI.Models.ListableItem("item 15"),
        };
        lcbc1.ViewModel = vm1;
        lcbc1.ListSelectionChanged += lcbc1_ListSelectionChanged;

        var vm2 = new LabelComboBoxControlViewModel();
        vm2.Label = "ComboBox 2";
        vm2.Items = new ObservableCollection<UI.Models.ListableItem>()
        {
            new UI.Models.ListableItem("Item 1", description:"Description 1"),
            new UI.Models.ListableItem("Item 2", description:"Description 2"),
            new UI.Models.ListableItem("Item 3", description:"Description 3"),
            new UI.Models.ListableItem("Item 4", description:"Description 4"),
        };
        vm2.ToolTip = "It's a simple tooltip for combobox label";
        vm2.SelectedItem = vm2.Items[2];
        lcbc2.ViewModel = vm2;
        lcbc2.ListSelectionChanged += lcbc2_ListSelectionChanged;

        var vm3 = new LabelComboBoxControlViewModel();
        vm3.Label = "ComboBox 3";
        vm3.IsStackedHorizontally = true;
        vm3.Items = new ObservableCollection<UI.Models.ListableItem>()
        {
            new UI.Models.ListableItem("Item 1", description:"Description 1"),
            new UI.Models.ListableItem("Item 2", description:"Description 2"),
            new UI.Models.ListableItem("Item 3", description:"Description 3"),
            new UI.Models.ListableItem("Item 4", description:"Description 4"),
        };
        vm3.ToolTip = "Horizontally stacked combobox!";
        vm3.SelectedItem = vm3.Items[1];
        lcbc3.ViewModel = vm3;
    }

    private void lcbc2_ListSelectionChanged(object? sender, UI.Models.Events.ListSelectionChangedEventArgs e)
    {
        txt2.Text = $"Selected item in cmbx2 changed to {e.SelectedItem}";
    }

    private void lcbc1_ListSelectionChanged(object? sender, UI.Models.Events.ListSelectionChangedEventArgs e)
    {
        txt1.Text = $"Selected index in cmbx1 changed to {e.SelectedIndex}";
    }
}