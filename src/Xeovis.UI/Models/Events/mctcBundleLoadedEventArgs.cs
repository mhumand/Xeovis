using System;
using Xeovis.Domain;

namespace Xeovis.UI.Models.Events
{
    public class mctcBundleLoadedEventArgs : EventArgs
    {
        public mctcBundle mctcBundle { get; set; }
        public object Tag { get; set; }

        public mctcBundleLoadedEventArgs(mctcBundle mctcBundle, object tag = null)
        {
            this.mctcBundle = mctcBundle;
            Tag = tag;
        }
    }
}
