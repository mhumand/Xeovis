using DatabaseReader.Definitions.UI;
using System;

namespace DatabaseReader.Events
{
    public class ChartDoubleClickedEventArgs : EventArgs
    {
        public ChartModel ChartModel {  get; set; }

        public ChartDoubleClickedEventArgs(ChartModel chartModel)
        {
            ChartModel = chartModel;
        }
    }
}
