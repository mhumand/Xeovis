using System.Drawing;

namespace Xeovis.UI.Models
{
    public class ListableItem
    {
        private string _label;
        public string Label => _label;

        private string _description;
        public string Description => _description;

        private int _index;
        public int Index => _index;

        private Color _color;
        public Color Color => _color;

        private bool _isVisible;
        public bool IsVisible => _isVisible;

        public bool CanBeRemoved { get; set; }

        private object _tag;
        public object Tag => _tag;

        public bool HasDescription => !string.IsNullOrWhiteSpace(_description);

        public ListableItem()
        {
            _isVisible = true;
            CanBeRemoved = false;
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

        public void SetLabel(string label)
        {
            _label = label;
        }

        public void SetDescription(string description)
        {
            _description = description;
        }

        public void SetIndex(int index)
        {
            _index = index;
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public void SetVisibility(bool visible)
        {
            _isVisible = visible;
        }

        public void SetTag(object tag)
        {
            _tag = tag;
        }

        public override string ToString()
        {
            return _label;
        }
    }
}
