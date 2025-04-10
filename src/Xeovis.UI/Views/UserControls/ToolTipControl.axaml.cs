using Avalonia.Controls;
using System.ComponentModel;

namespace Xeovis.UI;

public partial class ToolTipControl : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string toolTipText;

    public string ToolTipText
    {
        get { return toolTipText; }
        set
        {
            toolTipText = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToolTipText)));
        }
    }


    public ToolTipControl()
    {
        InitializeComponent();
        DataContext = this;
    }
}