using CommunityToolkit.Mvvm.ComponentModel;

namespace Xeovis.UI.ViewModels
{
    public partial class LabelControlViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? label;

        [ObservableProperty]
        private string? toolTip;

        [ObservableProperty]
        private bool isReadOnly;

        public bool HasToolTip => !string.IsNullOrWhiteSpace(ToolTip);
    }
}
