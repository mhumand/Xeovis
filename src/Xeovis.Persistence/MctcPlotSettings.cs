namespace Xeovis.Persistence
{
    public class mctcPlotSettings
    {
        public int ColorTransparencyIn100 { get; set; }
        public bool IsLegendVisible { get; set; }
        public mctcPlotSettings()
        {
            ColorTransparencyIn100 = 40;
            IsLegendVisible = true;
        }
    }
}
