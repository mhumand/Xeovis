using Avalonia.Controls;
using Avalonia.Threading;
using System;
using Xeovis.UI.Models.Events;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class LabelInputControl : UserControl
{
    private LabelInputControlViewModel _viewModel = new LabelInputControlViewModel();

    public LabelInputControlViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new LabelInputControlViewModel();
            DataContext = _viewModel;
        }
    }

    public string Value => _viewModel.Value;

    public event EventHandler<TextInputChangedEventArgs>? TextInputChanged;

    public LabelInputControl()
    {
        InitializeComponent();
        Focusable = true;
        btnClear.IsVisible = false;
        toolTip.IsVisible = false;
        Loaded += LabelInputControl_Loaded;
        tbInput.KeyDown += tbInput_KeyDown;
        tbInput.TextChanged += tbInput_TextChanged;
        tbInput.LostFocus += TbInput_LostFocus;
        txtBtnGrid.PointerEntered += txtBtnGrid_PointerEntered;
        txtBtnGrid.PointerExited += txtBtnGrid_PointerExited;
    }

    private void TbInput_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        tbInput.Text = tbInput.Text?.Trim();
        if (_viewModel.HasErrors)
            Dispatcher.UIThread.Post(() => tbInput.Focus(), DispatcherPriority.Input);
    }

    private void tbInput_TextChanged(object? sender, TextChangedEventArgs e)
    {
        TextInputChanged?.Invoke(this, new TextInputChangedEventArgs(tbInput.Text));
    }

    private void LabelInputControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        toolTip.IsVisible = _viewModel.HasToolTip;
        toolTip.ToolTipText = _viewModel.ToolTip;
    }

    private void txtBtnGrid_PointerExited(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        btnClear.IsVisible = false;
    }

    private void txtBtnGrid_PointerEntered(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        btnClear.IsVisible = !_viewModel.IsReadOnly;
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

    public void FocusAndSelectAll()
    {
        tbInput.Focus();
        tbInput.SelectAll();
    }
}