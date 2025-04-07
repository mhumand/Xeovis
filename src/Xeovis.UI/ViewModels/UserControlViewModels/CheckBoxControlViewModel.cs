using CommunityToolkit.Mvvm.ComponentModel;

namespace Xeovis.UI.ViewModels
{
    public partial class CheckBoxControlViewModel : LabelControlViewModel
    {
        [ObservableProperty]
        private string? description;

        [ObservableProperty]
        private bool isChecked;

        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

        public CheckBoxControlViewModel()
        {
            
        }

        public CheckBoxControlViewModel(string label, string description = null, bool isChecked = false) : this()
        {
            Label = label;
            Description = description;
            IsChecked = isChecked;
        }
    }
}
