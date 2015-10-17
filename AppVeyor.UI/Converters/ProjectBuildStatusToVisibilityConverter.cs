using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using AppVeyor.Common;
using AppVeyor.Common.Entities;
using AppVeyor.Common.Extensions;
using AppVeyor.UI.Converters.Base;

namespace AppVeyor.UI.Converters
{
    public class ProjectBuildStatusToVisibilityConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var project = value as Project;
            if (project != null && !project.Builds.IsEmpty())
            {
                var build = project.Builds.First();
                var status = build.Status.First();

                if (status.EqualsToStatus(BuildStatus.Queued) ||
                    status.EqualsToStatus(BuildStatus.Running))
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}