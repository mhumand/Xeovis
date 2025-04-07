using Avalonia.Controls;
using System;
using Xeovis.UI.Models.Events;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class LabelSpinnerControl : UserControl
{
    private LabelSpinnerControlViewModel _viewModel;

    public LabelSpinnerControlViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value;
            DataContext = value;
        }
    }


    public event EventHandler<SpinnerValueChangedEventArgs> ValueChanged;

    public LabelSpinnerControl()
    {
        InitializeComponent();
        Loaded += LabelSpinnerControl_Loaded;
        Focusable = true;
    }

    private void LabelSpinnerControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        toolTip.ToolTipText = _viewModel.ToolTip;
        toolTip.IsVisible = _viewModel.HasToolTip;
    }

    private void spinner_ValueChanged(object? sender, Avalonia.Controls.NumericUpDownValueChangedEventArgs e)
    {
        if (e.NewValue != null)
        {
            ValueChanged?.Invoke(sender, new SpinnerValueChangedEventArgs((int)e.NewValue));
        }
    }

    private void spinner_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Avalonia.Input.Key.Escape)
        {
            this.Focus();
        }
    }
}