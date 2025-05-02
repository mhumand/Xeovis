using Avalonia.Controls;

namespace Xeovis.UI;

public partial class ToolTipControl : UserControl
{
    private string toolTipText = string.Empty;

    public string ToolTipText
    {
        get { return toolTipText; }
        set
        {
            toolTipText = value;
            ToolTip.SetTip(image, value);
        }
    }

    public ToolTipControl()
    {
        InitializeComponent();
        DataContext = this;
    }
}