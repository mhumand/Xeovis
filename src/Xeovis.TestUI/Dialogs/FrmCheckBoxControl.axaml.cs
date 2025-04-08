using Avalonia.Controls;
using Xeovis.UI.ViewModels;

namespace Xeovis.TestUI;

public partial class FrmCheckBoxControl : Window
{
    public FrmCheckBoxControl()
    {
        InitializeComponent();
        DataContext = this;

        PrepareUI();
    }

    private void PrepareUI()
    {
        var vm1 = new CheckBoxControlViewModel();
        vm1.Label = "label1";
        vm1.ToolTip = "simple tooltip";
        vm1.IsChecked = true;
        cbcBox1.CheckStateChanged += cbcBox1_CheckStateChanged;

        var vm2 = new CheckBoxControlViewModel("label 2");
        cbcBox2.ViewModel = vm2;
        cbcBox2.CheckStateChanged += cbcBox2_CheckStateChanged;

        var vm3 = new CheckBoxControlViewModel("label 3", isChecked: true);
        cbcBox3.ViewModel = vm3;
        cbcBox3.CheckStateChanged += cbcBox3_CheckStateChanged;
    }

    private void cbcBox3_CheckStateChanged(object? sender, UI.Models.Events.CheckStateChangedEventArgs e)
    {
        txt3.Text = "Check Box 3: " + e.IsChecked.ToString();
    }

    private void cbcBox2_CheckStateChanged(object? sender, UI.Models.Events.CheckStateChangedEventArgs e)
    {
        txt2.Text = "Check Box 2: " + e.IsChecked.ToString();
    }

    private void cbcBox1_CheckStateChanged(object? sender, UI.Models.Events.CheckStateChangedEventArgs e)
    {
        txt1.Text = "Check Box 1: " + e.IsChecked.ToString();
    }
}