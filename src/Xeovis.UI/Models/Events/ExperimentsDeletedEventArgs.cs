using System;
using System.Collections.Generic;

namespace Xeovis.UI.Models.Events
{
    public class ExperimentsDeletedEventArgs : EventArgs
    {
        public List<Guid> ExperimentIds { get; set; }

        public ExperimentsDeletedEventArgs(List<Guid> experimentIds)
        {
            ExperimentIds = experimentIds;
        }

        public ExperimentsDeletedEventArgs(Guid experimentId)
        {
            ExperimentIds = new List<Guid> { experimentId };
        }
    }
}
