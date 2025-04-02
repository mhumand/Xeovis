using CommunityToolkit.Mvvm.ComponentModel;

namespace Xeovis.UI.ViewModels
{
    public partial class LabelSpinnerControlViewModel : LabelControlViewModel
    {
        [ObservableProperty]
        private int? value;

        [ObservableProperty]
        private int minValue;

        [ObservableProperty]
        private int maxValue;

        public LabelSpinnerControlViewModel()
        {
            minValue = 0;
        }

        public LabelSpinnerControlViewModel(string label, int value, int minValue, int maxValue)
        {
            Label = label;
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
