using System;
using System.ComponentModel.Design;
using AppVeyor.Commands.Base;
using AppVeyor.Common.Extensions;
using AppVeyor.UI.Services;
using Microsoft.VisualStudio.Shell;

namespace AppVeyor.Commands
{
    public class RefreshCommand : DynamicCommand
    {
        private static IServiceProvider _serviceProvider;

        public RefreshCommand(IServiceProvider serviceProvider) :
            base(serviceProvider, OnExecute, new CommandID(GuidList.guidShowRefresh,
                PackageCommands.cmdidShowRefresh))
        {
            _serviceProvider = serviceProvider;
        }

        protected override bool CanExecute(OleMenuCommand command)
        {
            return true;
        }

        private static void OnExecute(object sender, EventArgs e)
        {
            var eventManager = _serviceProvider.GetService<SEventManager, IEventManager>();
            eventManager.RefreshTriggerred();
        }
    }
}
