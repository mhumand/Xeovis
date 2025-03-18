namespace Xeovis.Persistence
{
    public class MctcPlotSettings
    {
        public int ColorTransparencyIn100 { get; set; }
        public bool IsLegendVisible { get; set; }
        public MctcPlotSettings()
        {
            ColorTransparencyIn100 = 40;
            IsLegendVisible = true;
        }
    }
}
