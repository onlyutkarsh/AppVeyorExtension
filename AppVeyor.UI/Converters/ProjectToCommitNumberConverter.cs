using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using AppVeyor.Common.Entities;
using AppVeyor.Common.Extensions;
using AppVeyor.UI.Converters.Base;

namespace AppVeyor.UI.Converters
{
    public class ProjectToCommitNumberConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var project = value as Project;
            if (project != null && !project.Builds.IsEmpty())
            {
                var build = project.Builds.First();
                return build.CommitId.Substring(0, 8);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}