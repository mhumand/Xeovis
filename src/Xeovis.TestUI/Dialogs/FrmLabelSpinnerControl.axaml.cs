using Avalonia.Controls;
using Xeovis.UI.ViewModels;

namespace Xeovis.TestUI;

public partial class FrmLabelSpinnerControl : Window
{
    public FrmLabelSpinnerControl()
    {
        InitializeComponent();
        DataContext = this;

        PrepareUI();
    }

    private void PrepareUI()
    {
        var vm1 = new LabelSpinnerControlViewModel("Test Label", 7, 2, 23);
        vm1.ToolTip = "Just a test tooltip!";
        lsc1.ViewModel = vm1;
        lsc1.ValueChanged += lsc1_ValueChanged;
    }

    private void lsc1_ValueChanged(object? sender, UI.Models.Events.SpinnerValueChangedEventArgs e)
    {
        txt1.Text = e.Value.ToString();
    }
}