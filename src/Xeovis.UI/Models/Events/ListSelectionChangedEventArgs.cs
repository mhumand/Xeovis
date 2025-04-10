using System;

namespace Xeovis.UI.Models.Events
{
    public class ListSelectionChangedEventArgs : EventArgs
    {
        public object SelectedItem { get; set; }
        public int SelectedIndex { get; set; }

        public ListSelectionChangedEventArgs(object selectedItem, int selectedIndex)
        {
            SelectedItem = selectedItem;
            SelectedIndex = selectedIndex;
        }
    }
}
