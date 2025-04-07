using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Events;

namespace Xeovis.UI.ViewModels
{
    public partial class LabelListControlViewModel : LabelControlViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<ListableItem> items;

        [ObservableProperty]
        private bool isMultiSelect;

        [ObservableProperty]
        private int selectedIndex;

        [ObservableProperty]
        private bool isCheckable;

        public event EventHandler<CheckStateChangedEventArgs> ItemCheckStateChanged;

        public LabelListControlViewModel()
        {
            selectedIndex = 0;
        }

        private void Items_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in Items)
            {
                item.CheckStateChanged -= Item_CheckStateChanged;
                item.CheckStateChanged += Item_CheckStateChanged;
            }
        }

        public LabelListControlViewModel(List<ListableItem> items) : this()
        {
            Items = new ObservableCollection<ListableItem>(items);
            foreach (var item in Items)
                item.CheckStateChanged += Item_CheckStateChanged;

            Items.CollectionChanged += Items_CollectionChanged;
        }

        private void Item_CheckStateChanged(object? sender, CheckStateChangedEventArgs e)
        {
            ItemCheckStateChanged?.Invoke(sender, e);
        }
    }
}
