using System;
using System.Globalization;
using System.Windows.Data;
using AppVeyor.Common.Entities;
using AppVeyor.Common.Extensions;
using AppVeyor.UI.Converters.Base;

namespace AppVeyor.UI.Converters
{
    public class ProjectToLastRunConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var project = value as Project;
            if (project != null)
            {
                return project.ToLastRunString(includeBuildDuration: true);
            }
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}