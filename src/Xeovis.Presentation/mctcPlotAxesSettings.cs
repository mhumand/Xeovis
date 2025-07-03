using Xeovis.Domain;

namespace Xeovis.Presentation
{
    public class mctcPlotAxesSettings
    {
        public xeosVariable YAxisVariable { get; set; }
        public xeosVariable XAxisVariable { get; set; }
        public mctcPlotAxisType YAxisType { get; set; }
        public mctcPlotAxisType XAxisType { get; set; }

        public mctcPlotAxesSettings()
        {
            YAxisVariable = new xeosVariable();
            XAxisVariable = new xeosVariable();
        }
        public mctcPlotAxesSettings(
            xeosVariable yAxisVariable,
            xeosVariable xAxisVariable,
            mctcPlotAxisType yAxisType,
            mctcPlotAxisType xAxisType) : this()
        {
            YAxisVariable = yAxisVariable;
            XAxisVariable = xAxisVariable;
            YAxisType = yAxisType;
            XAxisType = xAxisType;
        }
    }
}
