using CommunityToolkit.Mvvm.ComponentModel;

namespace Xeovis.UI.ViewModels
{
    public partial class CheckBoxControlViewModel : LabelControlViewModelBase
    {
        [ObservableProperty]
        private bool isChecked;

        public CheckBoxControlViewModel()
        {
            
        }

        public CheckBoxControlViewModel(string label, bool isChecked = false) : this()
        {
            Label = label;
            IsChecked = isChecked;
        }
    }
}
