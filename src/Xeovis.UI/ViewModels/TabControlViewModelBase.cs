using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Drawing;

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

        [ObservableProperty]
        private IBrush? tabColor;

        public Guid Id { get; set; }

        public Guid PresenterId { get; set; }

        public TabControlViewModelBase()
        {
            Header = "Default tab";
            IsVisible = true;
            CanBeRemoved = true;
            Id = Guid.NewGuid();
        }
    }
}
