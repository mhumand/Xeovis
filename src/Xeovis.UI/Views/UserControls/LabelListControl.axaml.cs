using Avalonia.Controls;
using System;
using System.ComponentModel;
using System.Linq;
using Xeovis.UI.Events;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class LabelListControl : UserControl, INotifyPropertyChanged
{
    private LabelListControlViewModel _viewModel;

    public event EventHandler<ListSelectionChangedEventArgs> ListSelectionChanged;

    public LabelListControl()
    {
        InitializeComponent();
        lbItems.KeyDown += lbItems_KeyDown;
        lbItems.SelectionChanged += lbItems_SelectionChanged;
    }

    private void lbItems_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        ListSelectionChanged?.Invoke(sender, new ListSelectionChangedEventArgs(lbItems.SelectedItem, lbItems.SelectedIndex));
    }

    private void lbItems_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Avalonia.Input.Key.Escape)
        {
            lbItems.SelectedIndex = -1;
        }
    }

    public LabelListControl(LabelListControlViewModel viewModel) : base()
    {
        SetDataContext(viewModel);
    }

    private void btnRemoveItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        var item = _viewModel.Items.FirstOrDefault(i => i.Tag == btn.Tag);
        if (item == null) return;
        _viewModel.Items.Remove(item);
    }

    public void SetDataContext(LabelListControlViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = viewModel;
    }
}