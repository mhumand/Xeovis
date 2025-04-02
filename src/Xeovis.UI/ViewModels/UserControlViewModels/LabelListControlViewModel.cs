using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels
{
    public partial class LabelListControlViewModel : LabelControlViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ListableItem> items;

        [ObservableProperty]
        private bool isMultiSelect;

        [ObservableProperty]
        private int selectedIndex;

        public LabelListControlViewModel()
        {
            selectedIndex = 0;
        }
    }
}
