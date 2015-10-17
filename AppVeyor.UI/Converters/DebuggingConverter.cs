using System;
using System.Windows.Data;
using AppVeyor.UI.Converters.Base;

namespace AppVeyor.UI.Converters
{
    public class DebuggingConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value; // Add the breakpoint here!!
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
