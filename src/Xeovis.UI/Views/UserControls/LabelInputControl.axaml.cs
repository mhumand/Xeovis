using Avalonia.Controls;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class LabelInputControl : UserControl
{
    public LabelInputControlViewModel _viewModel = new LabelInputControlViewModel();

    public LabelInputControl()
    {
        InitializeComponent();
        Focusable = true;
        btnClear.IsVisible = false;
        Loaded += LabelInputControl_Loaded;
        tbInput.KeyDown += tbInput_KeyDown;
        txtBtnGrid.PointerEntered += txtBtnGrid_PointerEntered;
        txtBtnGrid.PointerExited += txtBtnGrid_PointerExited;
    }

    private void LabelInputControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        btnClear.IsVisible = false;
        toolTip.IsVisible = _viewModel.HasToolTip;
        toolTip.ToolTipText = _viewModel.ToolTip;
    }

    private void txtBtnGrid_PointerExited(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        btnClear.IsVisible = false;
    }

    private void txtBtnGrid_PointerEntered(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        if (!_viewModel.IsReadOnly)
            btnClear.IsVisible = true;
    }

    public LabelInputControl(LabelInputControlViewModel viewModel = null) : this()
    {
        SetDataContext(viewModel);
    }

    public double GetDoubleValue()
    {
        return _viewModel.GetDoubleValue();
    }

    public void SetDataContext(LabelInputControlViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = viewModel;
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