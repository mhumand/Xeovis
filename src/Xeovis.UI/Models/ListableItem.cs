using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using Xeovis.UI.Models.Events;

namespace Xeovis.UI.Models
{
    public class ListableItem : INotifyPropertyChanged
    {
        public event EventHandler<CheckStateChangedEventArgs> CheckStateChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private string _label;
        public string Label
        {
            get { return _label; }
            set { _label = value; OnPropertyChanged(); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }

        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; OnPropertyChanged(); }
        }

        private Color _color;
        public Color Color
        {
            get { return _color; }
            set { _color = value; OnPropertyChanged(); }
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; OnPropertyChanged(); }
        }

        private object _tag;
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; OnPropertyChanged(); }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                CheckStateChanged?.Invoke(this, new CheckStateChangedEventArgs(value, this));
                OnPropertyChanged();
                _isChecked = value;
            }
        }

        public bool HasDescription => !string.IsNullOrWhiteSpace(_description);

        public ListableItem()
        {
            _isVisible = true;
            _id = Guid.NewGuid();
        }

        public ListableItem(object item) : this()
        {
            _label = item.ToString().Trim();
            _tag = item;
        }

        public ListableItem(string label, object tag = null, bool isVisible = true, string description = null) : this()
        {
            _label = label.Trim();
            _tag = tag;
            _isVisible = isVisible;
            _description = description;
        }

        public override string ToString()
        {
            return _label;
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ListableItem Copy(bool newId = false)
        {
            var item = new ListableItem();
            item.Label = _label;
            item.Description = _description;
            item.Tag = _tag;
            item.Id = newId ? Guid.NewGuid() : _id;
            item.Color = _color;
            item.IsVisible = _isVisible;
            item.IsChecked = _isChecked;
            item.Index = _index;
            return item;
        }
    }
}
