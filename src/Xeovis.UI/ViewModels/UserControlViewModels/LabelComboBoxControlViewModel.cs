using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels
{
    public partial class LabelComboBoxControlViewModel : LabelControlViewModel
    {
        [ObservableProperty]
        private ObservableCollection<ListableItem> items;

        [ObservableProperty]
        private int selectedIndex;

        public LabelComboBoxControlViewModel()
        {
            SelectedIndex = 0;
        }
    }
}
