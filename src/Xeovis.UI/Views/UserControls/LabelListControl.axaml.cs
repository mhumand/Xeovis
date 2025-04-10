using Avalonia.Controls;
using System;
using System.Linq;
using Xeovis.UI.Models.Events;
using Xeovis.UI.ViewModels;
using Xeovis.UI.Views.Helpers;

namespace Xeovis.UI;

public partial class LabelListControl : UserControl
{
    private LabelListControlViewModel _viewModel = new LabelListControlViewModel();

    public LabelListControlViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value;
            DataContext = value;
        }
    }

    public event EventHandler<ListSelectionChangedEventArgs> ListSelectionChanged;
    public event EventHandler<ListableItemRemovedEventArgs> ItemRemoved;

    public LabelListControl()
    {
        InitializeComponent();
        lbItems.SelectionChanged += lbItems_SelectionChanged;
        Loaded += LabelListControl_Loaded;
    }

    private void LabelListControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            toolTip.ToolTipText = _viewModel.ToolTip;
            toolTip.IsVisible = _viewModel.HasToolTip;
        }

        lbItems.ContextMenu = GetContextMenu();
    }

    private void lbItems_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        ListSelectionChanged?.Invoke(sender, new ListSelectionChangedEventArgs(lbItems.SelectedItem, lbItems.SelectedIndex));
    }

    private void btnRemoveItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_viewModel == null) return;
        if (sender is not Button btn) return;
        var item = _viewModel.Items.FirstOrDefault(i => i.Tag == btn.Tag);
        if (item == null) return;
        _viewModel.Items.Remove(item);
        ItemRemoved?.Invoke(this, new ListableItemRemovedEventArgs(item));
    }

    private ContextMenu? GetContextMenu()
    {
        var menu = new ContextMenu();

        var checkAll = new MenuItem();
        checkAll.Header = "Check All";
        checkAll.Click += CheckAll_Click;

        var uncheckAll = new MenuItem();
        uncheckAll.Header = "Uncheck All";
        uncheckAll.Click += CheckNone_Click;

        if (_viewModel.IsCheckable)
        {
            menu.Items.Add(checkAll);
            menu.Items.Add(uncheckAll);
        }

        var removeAll = new MenuItem();
        removeAll.Header = "Remove All";
        removeAll.Click += RemoveAll_Click;
        if (_viewModel.IsRemovable) menu.Items.Add(removeAll);

        return menu.ItemCount == 0 ? null : menu;
    }

    private void RemoveAll_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        foreach (var item in _viewModel.Items)
            ItemRemoved?.Invoke(this, new ListableItemRemovedEventArgs(item));

        _viewModel.Items.Clear();
    }

    private void CheckNone_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        foreach (var item in _viewModel.Items)
            item.IsChecked = false;
    }

    private void CheckAll_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        foreach (var item in _viewModel.Items)
            item.IsChecked = true;
    }
}