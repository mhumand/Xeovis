using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Xeovis.UI.Views.Helpers.Converters;

public class BoolToForegroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isTrue && isTrue)
            return Brushes.Black;
        
        return Brushes.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}