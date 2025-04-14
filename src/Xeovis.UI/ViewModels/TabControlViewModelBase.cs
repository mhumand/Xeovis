using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Xeovis.UI.ViewModels
{
    public partial class TabControlViewModelBase : ViewModelBase
    {
        [ObservableProperty]
        private string header;

        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private bool canBeRemoved;

        [ObservableProperty]
        private object? ownerTabControl;

        public Guid Id { get; set; }

        public TabControlViewModelBase()
        {
            Header = "Default tab";
            IsVisible = true;
            CanBeRemoved = true;
            Id = Guid.NewGuid();
        }
    }
}
