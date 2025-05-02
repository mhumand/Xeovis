using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels
{
    public partial class LabelComboBoxControlViewModel : LabelControlViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<ListableItem> items;

        [ObservableProperty]
        private ListableItem? selectedItem;

        [ObservableProperty]
        private bool isStackedHorizontally;

        public bool HasItems => Items != null && Items.Count > 0;

        public LabelComboBoxControlViewModel()
        {
            Items = new ObservableCollection<ListableItem>();
            IsStackedHorizontally = false;
        }
    }
}
