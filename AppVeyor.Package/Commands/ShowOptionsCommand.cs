using System;
using System.ComponentModel.Design;
using AppVeyor.Commands.Base;
using AppVeyor.Common;
using AppVeyor.UI.Options;
using Microsoft.VisualStudio.Shell;

namespace AppVeyor.Commands
{
    public class ShowOptionsCommand : DynamicCommand
    {
        public ShowOptionsCommand(IServiceProvider serviceProvider) 
            : base(serviceProvider, OnExecute, new CommandID(GuidList.guidShowOptions, PackageCommands.cmdidShowOptions))
        {
        }

        protected override bool CanExecute(OleMenuCommand command)
        {
            return true;
        }
        private static void OnExecute(object sender, EventArgs e)
        {
            Telemetry.Instance.TrackEvent("Options page opened");
            AppVeyorPackage.ShowOptionPage(typeof(AppVeyorOptions));
        }
    }
}
