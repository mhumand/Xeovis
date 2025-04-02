using System;

namespace Xeovis.UI.Events
{
    public class SpinnerValueChangedEventArgs : EventArgs
    {
        public int Value { get; set; }

        public SpinnerValueChangedEventArgs(int value)
        {
            Value = value;
        }
    }
}
