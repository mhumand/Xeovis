using System;

namespace Xeovis.UI.Models.Events
{
    public class TextInputChangedEventArgs : EventArgs
    {
        public string Text { get; set; }

        public object Tag { get; set; }

        public TextInputChangedEventArgs(string text, object tag = null)
        {
            Text = text;
            Tag = tag;
        }
    }
}
