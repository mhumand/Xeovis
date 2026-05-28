using CommunityToolkit.Mvvm.ComponentModel;

namespace Xeovis.UI.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        public object Tag { get; set; }
    }
}
