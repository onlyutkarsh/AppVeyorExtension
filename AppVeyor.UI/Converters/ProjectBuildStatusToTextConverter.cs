using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using AppVeyor.Common;
using AppVeyor.Common.Entities;
using AppVeyor.Common.Extensions;
using AppVeyor.UI.Converters.Base;

namespace AppVeyor.UI.Converters
{
    public class ProjectBuildStatusToTextConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var project = value as Project;
            if (project != null && !project.Builds.IsEmpty())
            {
                var build = project.Builds.First();
                var status = build.Status.First();


                if (status.EqualsToStatus(BuildStatus.Queued))
                {
                    return string.Format("QUEUED {0}", build.Created.GetHumanizedRunningFromTime());
                }
                if (status.EqualsToStatus(BuildStatus.Running))
                {
                    return string.Format("RUNNING {0}", build.Started.GetHumanizedRunningFromTime());
                }
            }
            return "";

        }

       

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}