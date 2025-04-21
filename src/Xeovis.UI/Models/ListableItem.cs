using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Drawing;
using Xeovis.UI.Models.Events;

namespace Xeovis.UI.Models
{
    public partial class ListableItem : ObservableObject
    {
        public event EventHandler<CheckStateChangedEventArgs> CheckStateChanged;

        [ObservableProperty]
        private Guid id;

        [ObservableProperty]
        private string label;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private int index;

        [ObservableProperty]
        private Color color;

        [ObservableProperty]
        private Color descriptionColor;

        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private object tag;

        [ObservableProperty]
        private Guid presenterId;

        [ObservableProperty]
        private bool isChecked;
        partial void OnIsCheckedChanged(bool oldValue, bool newValue) => CheckStateChanged?.Invoke(this, new CheckStateChangedEventArgs(newValue, this));

        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

        public ListableItem()
        {
            IsVisible = true;
            Id = Guid.NewGuid();
            Color = Color.Black;
            DescriptionColor = Color.Black;
        }

        public ListableItem(object item) : this()
        {
            Label = item.ToString().Trim();
            Tag = item;
        }

        public ListableItem(string label, Guid presenterId) : this()
        {
            Label = label;
            PresenterId = presenterId;
        }

        public ListableItem(string label, object tag = null, bool isVisible = true, string description = null) : this()
        {
            Label = label.Trim();
            Tag = tag;
            IsVisible = isVisible;
            Description = description;
        }

        public override string ToString()
        {
            return Label;
        }
    }
}
