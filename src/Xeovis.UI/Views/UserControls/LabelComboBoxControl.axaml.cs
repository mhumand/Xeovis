using Avalonia.Controls;
using System;
using System.Collections.Generic;
using Xeovis.UI.Models;
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
            if (_viewModel != null )
                comboBox.SelectedItem = _viewModel.SelectedItem;
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
        toolTip.ToolTipText = _viewModel.ToolTip;
        toolTip.IsVisible = _viewModel.HasToolTip;

        if (_viewModel.IsStackedHorizontally)
        {

        }
    }

    private void comboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        ListSelectionChanged?.Invoke(sender, new ListSelectionChangedEventArgs(comboBox.SelectedItem, comboBox.SelectedIndex));
    }

    public void SetItems(List<ListableItem> items)
    {
        _viewModel.Items = new System.Collections.ObjectModel.ObservableCollection<ListableItem>(items);
        if (_viewModel.Items.Count > 0)
        {
            _viewModel.SelectedItem = _viewModel.Items[0];
            comboBox.SelectedItem = _viewModel.Items[0];
        }
    }

    public void ClearItems()
    {
        _viewModel.Items.Clear();
    }
}