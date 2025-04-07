using CommunityToolkit.Mvvm.ComponentModel;

namespace Xeovis.UI.ViewModels
{
    public partial class LabelInputControlViewModel : LabelControlViewModelBase
    {
        [ObservableProperty]
        private string value;

        public LabelInputControlViewModel()
        {
            
        }

        public LabelInputControlViewModel(string label, string value, bool isReadOnly = false)
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
