using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using AppVeyor.Common.Extensions;
using AppVeyor.UI.Common;
using AppVeyor.UI.Converters.Base;

namespace AppVeyor.UI.Converters
{
    public class ErrorsToVisibilityConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var errors = value as ObservableCollection<Message>;

            if (errors.IsEmpty())
                return Visibility.Hidden;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}