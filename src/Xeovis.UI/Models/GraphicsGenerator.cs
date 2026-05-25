using System;
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

        public static Bitmap GetCross(int size, System.Drawing.Color fillColor)
        {
            double c = size / 2.0;
    
            var shape = new Path()
            {
                // Draw vertical line, then horizontal line
                Data = Geometry.Parse($"M {c},0 L {c},{size} M 0,{c} L {size},{c}"),
                Stroke = GetBrushFromColor(fillColor),
                StrokeThickness = Math.Max(1.5, size / 5.0), // Scales nicely with size
                StrokeLineCap = PenLineCap.Square
            };

            return RenderVisualToImage(shape, size, size);
        }
        
        public static Bitmap GetAsterisk(int size, System.Drawing.Color fillColor)
        {
            double c = size / 2.0;
            double offset = size * 0.15; // Padding for the diagonal lines
            double end = size * 0.85;
    
            var shape = new Path()
            {
                // Vertical, Horizontal, and both Diagonals
                Data = Geometry.Parse(
                    $"M {c},0 L {c},{size} " +               
                    $"M 0,{c} L {size},{c} " +               
                    $"M {offset},{offset} L {end},{end} " +  
                    $"M {offset},{end} L {end},{offset}"),   
                Stroke = GetBrushFromColor(fillColor),
                StrokeThickness = Math.Max(1.0, size / 8.0), // Slightly thinner lines to prevent crowding
                StrokeLineCap = PenLineCap.Round
            };

            return RenderVisualToImage(shape, size, size);
        }
        
        public static Color GetAvaloniaColor(System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static System.Drawing.Color GetColorFromAvaloniaColor(Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
