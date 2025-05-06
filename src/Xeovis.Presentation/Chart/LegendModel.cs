using System.Drawing;

namespace Xeovis.Presentation
{
    public class LegendModel
    {
        public LegendPosition Position { get; set; }
        public Color Background {  get; set; }
        public bool IsVertical {  get; set; }
        public bool IsVisible {  get; set; }

        public Color FontColor { get; set; }
        public float FontSize {  get; set; }
        public string? FontFamily { get; set; }

        public Color BorderColor { get; set; }
        public bool IsBorderVisible {  get; set; }
        public int BorderWidth { get; set; }

        public LegendModel()
        {
            Position = LegendPosition.InsideTopLeft;
            IsVisible = true;
            IsVertical = true;
        }
    }
}
