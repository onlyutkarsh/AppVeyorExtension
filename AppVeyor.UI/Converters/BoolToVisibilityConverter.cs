using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using AppVeyor.UI.Converters.Base;

namespace AppVeyor.UI.Converters
{
    class BoolToVisibilityConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool reversFlagSet = false;
            bool reverse = false;
            var inputParam = parameter as string;
            if (!string.IsNullOrWhiteSpace(inputParam))
            {
                reverse = string.Equals(inputParam, "reverse", StringComparison.CurrentCultureIgnoreCase);
            }
            
            var input = value as bool?;
            if (input != null)
            {
                reversFlagSet = input.Value;
            }
            if (reversFlagSet)
            {
                return reverse ? Visibility.Collapsed : Visibility.Visible;
            }
            return reverse ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }


}
