using System;
using System.Collections.Generic;

namespace Xeovis.UI.Models.Events
{
    public class mctcPlotsDeletedEventArgs : EventArgs
    {
        public List<Guid> PlotIds { get; }

        public mctcPlotsDeletedEventArgs(List<Guid> plotIds)
        {
            PlotIds = plotIds;
        }

        public mctcPlotsDeletedEventArgs(Guid plotId)
        {
            PlotIds = new List<Guid> { plotId };
        }
    }
}
