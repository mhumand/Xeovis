using System;

namespace Xeovis.WinControls
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
