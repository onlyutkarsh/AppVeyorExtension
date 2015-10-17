using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using AppVeyor.UI.Common;
using AppVeyor.UI.Converters.Base;

namespace AppVeyor.UI.Converters
{
    public class ErrorsToMessageVisibilityConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var errors = value as ObservableCollection<Message>;
            bool reverse = false;
            var inputParam = parameter as string;
            if (!string.IsNullOrWhiteSpace(inputParam))
            {
                reverse = string.Equals(inputParam, "reverse", StringComparison.CurrentCultureIgnoreCase);
            }
            if (reverse)
            {
                return errors != null && errors.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return errors != null && errors.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}