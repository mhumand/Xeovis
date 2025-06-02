using Avalonia.Data.Converters;
using Avalonia.Layout;
using System;
using System.Globalization;

namespace Xeovis.UI.Views.Helpers.Converters
{
    /// <summary>
    /// True = Horizontal orientation,
    /// False = Vertical orientation
    /// </summary>
    public class BoolToOrientationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = value is bool b && b;

            return flag ? Orientation.Horizontal : Orientation.Vertical;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
