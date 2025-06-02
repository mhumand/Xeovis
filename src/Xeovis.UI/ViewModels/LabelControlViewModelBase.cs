using CommunityToolkit.Mvvm.ComponentModel;

namespace Xeovis.UI.ViewModels
{
    public partial class LabelControlViewModelBase : ViewModelBase
    {
        [ObservableProperty]
        private string? label;

        [ObservableProperty]
        private string? toolTip = string.Empty;

        [ObservableProperty]
        private bool isReadOnly;

        [ObservableProperty]
        private bool isLabelVisible = true;

        public bool HasToolTip => !string.IsNullOrWhiteSpace(ToolTip);
    }
}
