using Avalonia.Controls;
using System;
using System.Linq;
using Xeovis.UI.Events;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class LabelListControl : UserControl
{
    private LabelListControlViewModel _viewModel = new LabelListControlViewModel();

    public event EventHandler<ListSelectionChangedEventArgs> ListSelectionChanged;

    public LabelListControl()
    {
        InitializeComponent();
        lbItems.SelectionChanged += lbItems_SelectionChanged;
    }

    private void lbItems_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        ListSelectionChanged?.Invoke(sender, new ListSelectionChangedEventArgs(lbItems.SelectedItem, lbItems.SelectedIndex));
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