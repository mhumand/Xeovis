using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Xeovis.UI.ViewModels
{
    public partial class CheckBoxControlViewModel : LabelControlViewModelBase
    {
        [ObservableProperty]
        private bool isChecked;

        [ObservableProperty]
        private Bitmap? image;

        public bool HasImage => image != null;

        public CheckBoxControlViewModel()
        {
            image = null;
        }

        public CheckBoxControlViewModel(string label, bool isChecked = false) : this()
        {
            Label = label;
            IsChecked = isChecked;
        }
    }
}
