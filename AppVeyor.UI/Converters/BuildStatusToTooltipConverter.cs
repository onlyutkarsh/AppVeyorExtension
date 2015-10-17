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
    public class BuildStatusToTooltipConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var project = value as Project;
            if (project != null && !project.Builds.IsEmpty())
            {
                var build = project.Builds.First();
                var status = build.Status.First();

                BuildStatus buildStatus;
                if (Enum.TryParse(status, true, out buildStatus))
                {
                    return string.Format("Build: {0}", buildStatus.ToString());
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}