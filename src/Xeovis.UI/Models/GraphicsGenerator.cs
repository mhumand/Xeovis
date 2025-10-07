using Avalonia.Controls;
using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media.Imaging;
using Avalonia.Media;
using Xeovis.UI.Views.Helpers.Converters;
using System.Collections.Generic;

namespace Xeovis.UI
{
    public static class GraphicsGenerator
    {
        private static Bitmap RenderVisualToImage(Control control, int width, int height)
        {
            var pixelSize = new PixelSize(width, height);
            var dpi = new Vector(96, 96);
            var renderTarget = new RenderTargetBitmap(pixelSize, dpi);

            //control.Measure(Size.Infinity);
            control.Arrange(new Rect(0, 0, width, height));
            //control.UpdateLayout(); // Ensure layout is calculated

            using (var ctx = renderTarget.CreateDrawingContext(true))
            {
                control.Render(ctx);
            }

            return renderTarget;
        }

        public static IBrush GetBrushFromColor(System.Drawing.Color color)
        {
            return (IBrush)new ColorConverter().Convert(color, typeof(IBrush), null, null); ;
        }

        public static Bitmap GetSquare(int size, System.Drawing.Color fillColor)
        {
            var shape = new Rectangle()
            {
                Width = size,
                Height = size,
                Fill = GetBrushFromColor(fillColor)
            };

            return RenderVisualToImage(shape, size, size);
        }


        public static Bitmap GetRectangle(int width, int height, System.Drawing.Color fillColor)
        {
            var shape = new Rectangle()
            {
                Width = width,
                Height = height,
                Fill = GetBrushFromColor(fillColor)
            };

            return RenderVisualToImage(shape, width, height);
        }

        public static Bitmap GetCircle(int size, System.Drawing.Color fillColor)
        {
            var shape = new Ellipse()
            {
                Width = size,
                Height = size,
                Fill = GetBrushFromColor(fillColor)
            };

            return RenderVisualToImage(shape, size, size);
        }

        public static Bitmap GetTriangleUp(int size, System.Drawing.Color fillColor)
        {
            var shape = new Polygon()
            {
                Points = new List<Point>()
                {
                    new Point(size / 2, 0),
                    new Point(0, size),
                    new Point(size, size)
                },
                Fill = GetBrushFromColor(fillColor)
            };

            return RenderVisualToImage(shape, size, size);
        }

        public static Bitmap GetTriangleDown(int size, System.Drawing.Color fillColor)
        {
            var shape = new Polygon()
            {
                Points = new List<Point>()
                {
                    new Point(0, 0),
                    new Point(size, 0),
                    new Point(size / 2, size)
                },
                Fill = GetBrushFromColor(fillColor)
            };

            return RenderVisualToImage(shape, size, size);
        }

        public static Bitmap GetDiamond(int size, System.Drawing.Color fillColor)
        {
            var shape = new Polygon()
            {
                Points = new List<Point>()
                {
                    new Point(size / 2, 0),
                    new Point(0, size / 2),
                    new Point(size / 2, size),
                    new Point(size, size / 2)
                },
                Fill = GetBrushFromColor(fillColor)
            };

            return RenderVisualToImage(shape, size, size);
        }
    }
}
