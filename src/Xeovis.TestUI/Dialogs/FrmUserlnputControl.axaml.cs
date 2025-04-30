using Avalonia.Controls;
using Xeovis.Base.Validations;
using Xeovis.UI.ViewModels;

namespace Xeovis.TestUI;

public partial class FrmUserInputControl : Window
{
    public FrmUserInputControl()
    {
        InitializeComponent();
        PrepareUI();
    }

    private void PrepareUI()
    {
        var vmNumber = new LabelInputControlViewModel("number", "266.6");
        vmNumber.ToolTip = "It's a simple tooltip";
        vmNumber.ValidationRules.Add(new NumericRule(100, 1000));
        licNumber.ViewModel = vmNumber;
        licNumber.TextInputChanged += LicNumber_TextInputChanged;

        var vmSimpleText = new LabelInputControlViewModel("Simple Text", "It's a simple text.");
        vmSimpleText.ValidationRules.Add(new NotEmptyOrWhitespaceRule());
        licSimpleText.ViewModel = vmSimpleText;

        var vmSimpleTextReadOnly = new LabelInputControlViewModel("Read-only Text", "read-only text", true);
        licSimpleTextReadOnly.ViewModel = vmSimpleTextReadOnly;

        var vmDuplicate = new LabelInputControlViewModel("Duplicate name test (Bundle 1)", "Bundle");
        vmDuplicate.ValidationRules.Add(new DuplicateNameRule(new System.Collections.Generic.List<string>() { "Bundle 1", "Bundle 2" }));
        licDuplicateRule.ViewModel = vmDuplicate;
    }

    private void LicNumber_TextInputChanged(object? sender, UI.Models.Events.TextInputChangedEventArgs e)
    {
        txtTest1.Text = licNumber.ViewModel.GetDoubleValue().ToString();
    }
}