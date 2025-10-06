namespace Xeovis.Presentation
{
    public enum mctcPlotPointsGroupingType
    {
        Batch,
        Experiment,
        Temperature,
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
