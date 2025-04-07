using Avalonia.Controls;
using System;
using Xeovis.UI.Models.Events;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class LabelInputControl : UserControl
{
    private LabelInputControlViewModel _viewModel;

    public LabelInputControlViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value;
            DataContext = value;
        }
    }

    public event EventHandler<TextInputChangedEventArgs> TextInputChanged;

    public LabelInputControl()
    {
        InitializeComponent();
        Focusable = true;
        btnClear.IsVisible = false;
        Loaded += LabelInputControl_Loaded;
        tbInput.KeyDown += tbInput_KeyDown;
        tbInput.TextChanged += tbInput_TextChanged;
        txtBtnGrid.PointerEntered += txtBtnGrid_PointerEntered;
        txtBtnGrid.PointerExited += txtBtnGrid_PointerExited;
    }

    private void tbInput_TextChanged(object? sender, TextChangedEventArgs e)
    {
        TextInputChanged?.Invoke(this, new TextInputChangedEventArgs(tbInput.Text));
    }

    private void LabelInputControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        btnClear.IsVisible = false;
        if (_viewModel != null)
        {
            toolTip.IsVisible = _viewModel.HasToolTip;
            toolTip.ToolTipText = _viewModel.ToolTip;
        }
    }

    private void txtBtnGrid_PointerExited(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        btnClear.IsVisible = false;
    }

    private void txtBtnGrid_PointerEntered(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        if (_viewModel != null && !_viewModel.IsReadOnly)
            btnClear.IsVisible = true;
    }

    private void btnClear_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        tbInput.Clear();
        tbInput.Focus();
    }

    private void tbInput_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Avalonia.Input.Key.Escape || e.Key == Avalonia.Input.Key.Enter)
        {
            this.Focus();
        }
    }
}