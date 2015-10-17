using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using AppVeyor.Common;
using AppVeyor.Common.Entities;
using AppVeyor.Common.Extensions;
using AppVeyor.UI.Converters.Base;

namespace AppVeyor.UI.Converters
{
    public class ProjectBuildStatusToColorConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var success = new SolidColorBrush(Color.FromRgb(21, 194, 60));
            var failed = new SolidColorBrush(Colors.OrangeRed);
            var queued = new SolidColorBrush(Colors.DeepSkyBlue);
            var running = new SolidColorBrush(Color.FromRgb(255,204,0));
            var unknown = new SolidColorBrush(Colors.DimGray);
            var project = value as Project;
            if (project != null && !project.Builds.IsEmpty())
            {
                var build = project.Builds.First();
                var status = build.Status.First();
                if (status.EqualsToStatus(BuildStatus.Success))
                {
                    return success;
                }
                if (status.EqualsToStatus(BuildStatus.Failed))
                {
                    return failed;
                }
                if (status.EqualsToStatus(BuildStatus.Queued))
                {
                    return queued;
                }
                if (status.EqualsToStatus(BuildStatus.Running))
                {
                    return running;
                }
            }
            return unknown;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
