using Avalonia.Markup.Xaml.Converters;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.Models.Events;

namespace Xeovis.UI.ViewModels
{
    public partial class LabelListControlViewModel : LabelControlViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<ListableItem> items;

        [ObservableProperty]
        private ItemSelectionMode selectionMode;

        [ObservableProperty]
        private int selectedIndex;

        [ObservableProperty]
        private bool isCheckable;

        [ObservableProperty]
        private bool isRemovable;

        [ObservableProperty]
        private bool canChangeOrder;
        
        public event EventHandler<CheckStateChangedEventArgs> ItemCheckStateChanged;

        public LabelListControlViewModel()
        {
            selectedIndex = 0;
            IsCheckable = false;
            IsRemovable = false;
            SelectionMode = ItemSelectionMode.SingleToggle;
            Items = new ObservableCollection<ListableItem>();
        }

        public LabelListControlViewModel(List<ListableItem> items) : this()
        {
            SetItems(items);
        }

        public void SetItems(List<ListableItem> items)
        {
            Items.Clear();
            if (items == null) return;
            Items = new ObservableCollection<ListableItem>(items);
            foreach (var item in Items)
                item.CheckStateChanged += Item_CheckStateChanged;

            Items.CollectionChanged += Items_CollectionChanged;
        }

        private void Item_CheckStateChanged(object? sender, CheckStateChangedEventArgs e)
        {
            ItemCheckStateChanged?.Invoke(sender, e);
        }

        private void Items_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in Items)
            {
                item.CheckStateChanged -= Item_CheckStateChanged;
                item.CheckStateChanged += Item_CheckStateChanged;
            }
        }
    }
}
