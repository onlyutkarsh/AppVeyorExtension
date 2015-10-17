using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using AppVeyor.Common.Entities;
using AppVeyor.Common.Extensions;
using AppVeyor.UI.Converters.Base;
using Microsoft.VisualStudio.Shell;

namespace AppVeyor.UI.Converters
{
    public class ButtonForegroundColorConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null)
            {
                var project = values[0] as Project;
                var control = values[1] as Button;
                var param = parameter as string;
                if (control != null && project != null && !string.IsNullOrEmpty(param))
                {
                    switch (param)
                    {
                        case "START_BUILD":
                        {
                            if (project.IsInProgress())
                            {
                                control.IsEnabled = false;
                                control.SetResourceReference(Control.ForegroundProperty, VsBrushes.GrayTextKey);
                            }
                            else
                            {
                                control.IsEnabled = true;
                                control.SetResourceReference(Control.ForegroundProperty, VsBrushes.ButtonTextKey);
                            }
                            break;
                        }
                        case "CANCEL_BUILD":
                        {
                            if (project.IsInProgress())
                            {
                                control.IsEnabled = true;
                                control.SetResourceReference(Control.ForegroundProperty, VsBrushes.ButtonTextKey);
                            }
                            else
                            {
                                control.IsEnabled = false;
                                control.SetResourceReference(Control.ForegroundProperty, VsBrushes.GrayTextKey);
                            }
                            break;
                        }
                    }
                    return null;
                }
            }   
            return VsBrushes.ButtonTextKey;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
