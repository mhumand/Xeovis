using System;

namespace Xeovis.UI.Models.Events
{
    public class ListableItemRemovedEventArgs : EventArgs
    {
        public ListableItem Item { get; set; }

        public ListableItemRemovedEventArgs(ListableItem item)
        {
            Item = item;
        }
    }
}
