using Avalonia.Controls;
using Xeovis.UI.ViewModels;

namespace Xeovis.TestUI;

public partial class FrmUserInputControl : Window
{
    private LabelInputControlViewModel _vmSimpleText;
    private LabelInputControlViewModel _vmSimpleTextReadOnly;
    private LabelInputControlViewModel _vmNumber;

    public FrmUserInputControl()
    {
        InitializeComponent();

        PrepareUI();
    }

    private void PrepareUI()
    {
        _vmNumber = new LabelInputControlViewModel("number", 266.6);
        _vmNumber.ToolTip = "It's a simple tooltip";
        licNumber.ViewModel = _vmNumber;

        _vmSimpleText = new LabelInputControlViewModel("Simple Text", "It's a simple text.");
        licSimpleText.ViewModel = _vmSimpleText;

        _vmSimpleTextReadOnly = new LabelInputControlViewModel("Read-only Text", "read-only text", true);
        licSimpleTextReadOnly.ViewModel = _vmSimpleTextReadOnly;
    }
}