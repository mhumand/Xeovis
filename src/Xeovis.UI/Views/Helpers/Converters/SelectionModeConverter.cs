using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Globalization;
using Xeovis.UI.Models.Enums;

namespace Xeovis.UI.Views.Helpers.Converters
{
    internal class SelectionModeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ItemSelectionMode mode)
            {
                return mode switch
                {
                    ItemSelectionMode.Single => SelectionMode.Single,
                    ItemSelectionMode.SingleToggle => SelectionMode.Single | SelectionMode.Toggle,
                    ItemSelectionMode.Multiple => SelectionMode.Multiple,
                    ItemSelectionMode.MultipleToggle => SelectionMode.Multiple | SelectionMode.Toggle,
                    ItemSelectionMode.AlwaysSelected => SelectionMode.AlwaysSelected,
                    _ => SelectionMode.Toggle,
                };
            }

            return SelectionMode.Toggle;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
