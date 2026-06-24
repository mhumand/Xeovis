using Avalonia.Controls;
using System;
using System.Linq;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Events;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class LabelListControl : UserControl
{
    private LabelListControlViewModel _viewModel = new LabelListControlViewModel();

    public LabelListControlViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new LabelListControlViewModel();
            DataContext = _viewModel;
        }
    }

    public event EventHandler<ListSelectionChangedEventArgs>? ListSelectionChanged;
    public event EventHandler<ListableItemRemovedEventArgs>? ItemRemoved;

    public LabelListControl()
    {
        InitializeComponent();
        lbItems.SelectionChanged += lbItems_SelectionChanged;
        lbItems.KeyDown += lbItems_KeyDown;
        Loaded += LabelListControl_Loaded;
        toolTip.IsVisible = false;
    }

    private void LabelListControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        toolTip.ToolTipText = _viewModel.ToolTip;
        toolTip.IsVisible = _viewModel.HasToolTip;
        lbItems.ContextRequested += lbItemsOnContextRequested;
    }

    private void lbItemsOnContextRequested(object? sender, ContextRequestedEventArgs e)
    {
        if (lbItems.SelectedItem is not ListableItem item) return;
        
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
        if (_viewModel.IsRemovable)
            menu.Items.Add(removeAll);

        if (_viewModel.CanChangeOrder)
        {
            menu.Items.Add(new Separator());
            var itemIndex = _viewModel.Items.IndexOf(item);
            
            var moveUp = new MenuItem();
            moveUp.Header = "Move Up";
            moveUp.Click += (s, e) => MoveItemUp(item);
            
            if (itemIndex > 0)
                menu.Items.Add(moveUp);
            
            var moveDown = new MenuItem();
            moveDown.Header = "Move Down";
            moveDown.Click += (s, e) => MoveItemDown(item);
            
            if (itemIndex < _viewModel.Items.Count - 1)
                menu.Items.Add(moveDown);
        }
        
        if (menu.Items.Count == 0) return;
        menu.Open(this);
    }

    public void MoveItemUp(ListableItem item)
    {
        if (item == null) return;
        var itemIndex = _viewModel.Items.IndexOf(item);
        _viewModel.Items.Move(itemIndex, itemIndex - 1);
        _viewModel.SelectedIndex = itemIndex - 1;
    }

    public void MoveItemDown(ListableItem item)
    {
        if (item == null) return;
        var itemIndex = _viewModel.Items.IndexOf(item);
        _viewModel.Items.Move(itemIndex, itemIndex + 1);
        _viewModel.SelectedIndex = itemIndex + 1;
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

    private void RemoveAll_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var items = _viewModel.Items.ToList();
        _viewModel.Items.Clear();
        foreach (var item in items)
            ItemRemoved?.Invoke(this, new ListableItemRemovedEventArgs(item));
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

    private void lbItems_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Avalonia.Input.Key.Escape)
            lbItems.SelectedIndex = -1;
    }

    public ListableItem GetSelectedItem()
    {
        return lbItems.SelectedItem as ListableItem;
    }
}