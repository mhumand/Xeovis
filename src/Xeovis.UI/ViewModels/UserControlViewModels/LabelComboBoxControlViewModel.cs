using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels
{
    public partial class LabelComboBoxControlViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string label;

        [ObservableProperty]
        private ObservableCollection<ListableItem> items;

        [ObservableProperty]
        private string toolTip;

        [ObservableProperty]
        private int selectedIndex;

        public bool HasToolTip => !string.IsNullOrWhiteSpace(toolTip);

        public LabelComboBoxControlViewModel()
        {
            selectedIndex = 0;
        }
    }
}
