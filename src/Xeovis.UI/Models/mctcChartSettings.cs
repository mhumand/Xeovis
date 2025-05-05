namespace Xeovis.UI.Models
{
    public class mctcChartSettings
    {
        public bool ShowAxesTitle { get; set; }
        public bool ShowChartTitle { get; set; }

        public bool LockAxesLimits { get; set; }
        public bool UseAxesLimits { get; set; }
        public double AxesMinLimit { get; set; }
        public double AxesMaxLimit { get; set; }

        public mctcChartSettings()
        {
            ShowAxesTitle = true;
            ShowChartTitle = true;
            LockAxesLimits = false;
            UseAxesLimits = false;
            AxesMinLimit = 0;
            AxesMaxLimit = 1;
        }

        public mctcChartSettings Copy()
        {
            var copy = new mctcChartSettings();
            copy.ShowChartTitle = ShowChartTitle;
            copy.ShowAxesTitle = ShowAxesTitle;
            copy.LockAxesLimits = LockAxesLimits;
            copy.UseAxesLimits = UseAxesLimits;
            copy.AxesMinLimit = AxesMinLimit;
            copy.AxesMaxLimit = AxesMaxLimit;
            return copy;
        }
    }
}
