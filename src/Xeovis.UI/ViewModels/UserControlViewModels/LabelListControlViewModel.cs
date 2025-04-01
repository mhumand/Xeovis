using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels
{
    public partial class LabelListControlViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string label;

        [ObservableProperty]
        private ObservableCollection<ListableItem> items;

        [ObservableProperty]
        private bool isMultiSelect;

        public LabelListControlViewModel()
        {
            
        }

    }
}
