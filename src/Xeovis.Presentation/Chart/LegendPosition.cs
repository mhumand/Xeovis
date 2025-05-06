namespace Xeovis.Presentation
{
    public enum LegendPosition
    {
        InsideTopLeft,
        InsideTopRight,
        InsideTopCenter,
        InsideBottomLeft,
        InsideBottomRight,
        InsideBottomCenter,
        InsideLeftCenter,
        InsideRightCenter,
        OutsideTopLeft,
        OutsideTopRight,
        OutsideTopCenter,
        OutsideBottomLeft,
        OutsideBottomRight,
        OutsideBottomCenter,
        OutsideLeftTop,
        OutsideLeftBottom,
        OutsideLeftCenter,
        OutsideRightTop,
        OutsideRightBottom,
        OutsideRightCenter
    }

    public static class LegendPositionExtensions
    {
        public static bool IsInside(this LegendPosition position)
        {
            return position.ToString().StartsWith("inside", System.StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
