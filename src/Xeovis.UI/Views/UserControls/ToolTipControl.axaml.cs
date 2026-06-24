using Avalonia;
using Avalonia.Controls;

namespace Xeovis.UI;

public partial class ToolTipControl : UserControl
{
// 1. Register an Avalonia StyledProperty so we can use {Binding} from the parent!
    public static readonly StyledProperty<string> ToolTipTextProperty =
        AvaloniaProperty.Register<ToolTipControl, string>(nameof(ToolTipText), string.Empty);

    // 2. Standard CLR wrapper pointing to the Avalonia property
    public string ToolTipText
    {
        get => GetValue(ToolTipTextProperty);
        set => SetValue(ToolTipTextProperty, value);
    }

    public ToolTipControl()
    {
        InitializeComponent();
    }
    
    // 3. React to binding updates and pass the string to the internal image
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ToolTipTextProperty)
        {
            ToolTip.SetTip(this, change.NewValue as string);
        }
    }
}