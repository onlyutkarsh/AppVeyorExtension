using System;
using System.Globalization;
using System.Windows.Data;
using AppVeyor.UI.Converters.Base;

namespace AppVeyor.UI.Converters
{
    public class MessagesHeaderConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int errorCount = 0;
            if (value != null && !string.IsNullOrEmpty(value.ToString()) && int.TryParse(value.ToString(), out errorCount))
            {
                return string.Format("Errors ({0})", errorCount);
            }
            return "Errors";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}