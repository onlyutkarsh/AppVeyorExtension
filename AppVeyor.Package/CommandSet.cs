using System;
using System.ComponentModel.Design;
using AppVeyor.Commands;
using Microsoft.VisualStudio.Shell;
using AppVeyor.Common.Extensions;
using AppVeyor.UI.Services;

namespace AppVeyor
{
    public class CommandSet
    {
        private IServiceProvider _serviceProvider;
        private OleMenuCommandService _menuCommandService;

        public CommandSet(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _menuCommandService = _serviceProvider.GetService<IMenuCommandService, OleMenuCommandService>();
        }
        public void Initialize()
        {
            RegisterMenuCommands();
        }

        private void RegisterMenuCommands()
        {
            ICommandManagerService commandManager = _serviceProvider.GetService<SCommandManagerService, ICommandManagerService>();
            
            var showAppVeyorWindowCommand = new ShowAppVeyorWindowCommand(_serviceProvider);
            _menuCommandService.AddCommand(showAppVeyorWindowCommand);
            commandManager.RegisterCommand(showAppVeyorWindowCommand);

            var showOptionsCommand = new ShowOptionsCommand(_serviceProvider);
            _menuCommandService.AddCommand(showOptionsCommand);
            commandManager.RegisterCommand(showOptionsCommand);

            var refrshCommand = new RefreshCommand(_serviceProvider);
            _menuCommandService.AddCommand(refrshCommand);
            commandManager.RegisterCommand(refrshCommand);

        }
    }
}