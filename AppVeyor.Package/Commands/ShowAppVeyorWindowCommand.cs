using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Windows;
using AppVeyor.Commands.Base;
using AppVeyor.Common;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace AppVeyor.Commands
{
    public class ShowAppVeyorWindowCommand : DynamicCommand
    {
        public ShowAppVeyorWindowCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, OnExecute, new CommandID(GuidList.guidShowAppVeyorWindow, 
                PackageCommands.cmdidShowAppVeyorWindow))
        {
        }

        protected override bool CanExecute(OleMenuCommand command)
        {
            return true;
        }

        private static void OnExecute(object sender, EventArgs e)
        {
            try
            {
                ToolWindowPane window = AppVeyorPackage.ToolWindow;
                if ((window == null) || (window.Frame == null))
                {
                    throw new COMException("Cannot create toolwindow");
                }

                IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
                ErrorHandler.ThrowOnFailure(windowFrame.Show());
                Telemetry.Instance.TrackEvent("Toolwindow opened from menu");
            }
            catch (Exception exception)
            {
                Telemetry.Instance.TrackException(exception);
                MessageBox.Show("Sorry, Unable to open AppVeyor window.", "AppVeyor Extension for Visual Studio",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
