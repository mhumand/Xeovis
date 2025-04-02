using Avalonia.Controls;
using System;
using Xeovis.UI.Events;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class LabelComboBoxControl : UserControl
{
    private LabelComboBoxControlViewModel _viewModel = new LabelComboBoxControlViewModel();

    public event EventHandler<ListSelectionChangedEventArgs> ListSelectionChanged;

    public LabelComboBoxControl()
    {
        InitializeComponent();
        Loaded += LabelComboBoxControl_Loaded;
    }

    private void LabelComboBoxControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        toolTip.ToolTipText = _viewModel.ToolTip;
        toolTip.IsVisible = _viewModel.HasToolTip;
    }

    public void SetDataContext(LabelComboBoxControlViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = viewModel;
    }

    private void comboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        ListSelectionChanged?.Invoke(sender, new ListSelectionChangedEventArgs(comboBox.SelectedItem, comboBox.SelectedIndex));
    }
}