using CommunityToolkit.Mvvm.ComponentModel;

namespace Xeovis.UI.ViewModels
{
    public partial class LabelInputControlViewModel : ViewModelBase
    {

        [ObservableProperty]
        private string label;

        [ObservableProperty]
        private object value;

        [ObservableProperty]
        private bool isReadOnly;

        [ObservableProperty]
        private string toolTip;

        public bool HasToolTip => !string.IsNullOrWhiteSpace(ToolTip);

        public LabelInputControlViewModel()
        {
            
        }

        public LabelInputControlViewModel(string Label, object Value, bool IsReadOnly = false)
        {
            label = Label;
            value = Value;
            isReadOnly = IsReadOnly;
        }

        public double GetDoubleValue()
        {
            return Base.DoubleExtensions.GetDoubleValueFromText(Value.ToString());
        }
    }
}
