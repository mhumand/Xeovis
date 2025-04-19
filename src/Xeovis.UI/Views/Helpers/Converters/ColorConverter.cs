using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Xeovis.UI.Views.Helpers.Converters
{
    public class ColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is System.Drawing.Color color)
            {
                return ColorToBrushConverter.Convert(Color.FromArgb(color.A,color.R, color.G, color.B), typeof(IBrush));
            }

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
