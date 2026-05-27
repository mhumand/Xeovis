using System.ComponentModel;

namespace Xeovis.Presentation
{
    public enum mctcPlotPointsGroupingType
    {
        [Description("Batch")]
        Batch,
        
        [Description("Experiment")]
        Experiment,
        
        [Description("Experimental Run")]
        ExperimentalRun,
        
        [Description("Temperature")]
        Temperature,
        
        [Description("Pressure")]
        Pressure
    }

    public static class mctcPlotPointsGroupingTypeExtensions
    {
        public static bool IsNumber(this mctcPlotPointsGroupingType type)
        {
            return type is mctcPlotPointsGroupingType.Temperature or mctcPlotPointsGroupingType.Pressure;
        }
    }
}
