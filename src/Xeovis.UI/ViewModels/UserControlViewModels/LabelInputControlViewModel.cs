using CommunityToolkit.Mvvm.ComponentModel;

namespace Xeovis.UI.ViewModels
{
    public partial class LabelInputControlViewModel : LabelControlViewModel
    {
        [ObservableProperty]
        private object value;

        public LabelInputControlViewModel()
        {
            
        }

        public LabelInputControlViewModel(string label, object value, bool isReadOnly = false)
        {
            Label = label;
            Value = value;
            IsReadOnly = isReadOnly;
        }

        public double GetDoubleValue()
        {
            return Base.DoubleExtensions.GetDoubleValueFromText(Value.ToString());
        }
    }
}
