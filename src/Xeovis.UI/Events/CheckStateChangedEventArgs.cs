using System;

namespace Xeovis.UI.Events
{
    public class CheckStateChangedEventArgs : EventArgs
    {
        public bool IsChecked { get; set; }
        public object Tag { get; set; }

        public CheckStateChangedEventArgs(bool isChecked, object tag = null)
        {
            IsChecked = isChecked;
            Tag = tag;
        }
    }
}
