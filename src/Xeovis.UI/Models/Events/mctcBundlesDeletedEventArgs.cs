using System;
using System.Collections.Generic;

namespace Xeovis.UI.Models.Events
{
    public class mctcBundlesDeletedEventArgs : EventArgs
    {
        public List<Guid> BundleIds { get; set; }

        public mctcBundlesDeletedEventArgs(List<Guid> bundleIds)
        {
            BundleIds = bundleIds;
        }

        public mctcBundlesDeletedEventArgs(Guid bundleId)
        {
            BundleIds = new List<Guid> { bundleId };
        }
    }
}
