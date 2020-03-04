using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace InventoryTracker.Helpers
{
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bValue = false;
            if (value is bool b1)
            {
                bValue = b1;
            }

            var tmp = value as bool?;
            if (tmp != null)
            {
                bValue = tmp.Value;
            }

            if (parameter == null)
                return bValue ? Visibility.Visible : Visibility.Collapsed;
            else
                return !bValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                return (Visibility)value == Visibility.Visible;
            }
            else
            {
                return false;
            }
        }
    }
}
