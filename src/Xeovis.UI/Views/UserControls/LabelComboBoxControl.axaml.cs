using Avalonia.Controls;
using System;
using Xeovis.UI.Models.Events;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class LabelComboBoxControl : UserControl
{
    private LabelComboBoxControlViewModel _viewModel = new LabelComboBoxControlViewModel();

    public LabelComboBoxControlViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value;
            DataContext = value;
        }
    }

    public event EventHandler<ListSelectionChangedEventArgs> ListSelectionChanged;

    public LabelComboBoxControl()
    {
        InitializeComponent();
        Loaded += LabelComboBoxControl_Loaded;
    }

    private void LabelComboBoxControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            toolTip.ToolTipText = _viewModel.ToolTip;
            toolTip.IsVisible = _viewModel.HasToolTip;
        }
    }

    private void comboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        ListSelectionChanged?.Invoke(sender, new ListSelectionChangedEventArgs(comboBox.SelectedItem, comboBox.SelectedIndex));
    }
}