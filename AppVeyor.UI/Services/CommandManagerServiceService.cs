using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Shell;

namespace AppVeyor.UI.Services
{
    public class CommandManagerServiceService : ICommandManagerService, SCommandManagerService
    {
        private IServiceProvider _serviceProvider;
        private readonly IList<OleMenuCommand> _registeredCommands;

        public CommandManagerServiceService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _registeredCommands = new List<OleMenuCommand>();

        }
        public void RegisterCommand(OleMenuCommand command)
        {
            if (_registeredCommands.SingleOrDefault(
                cmd => cmd.CommandID.Guid.Equals(command.CommandID.Guid) &&
                    cmd.CommandID.ID.Equals(command.CommandID.ID)) == null)
            {
                _registeredCommands.Add(command);
            }
        }

        public void UnRegisterCommand(OleMenuCommand command)
        {
            if (_registeredCommands.SingleOrDefault(
               cmd => cmd.CommandID.Guid.Equals(command.CommandID.Guid) &&
                   cmd.CommandID.ID.Equals(command.CommandID.ID)) != null)
            {
                _registeredCommands.Remove(command);
            }
        }
    }
}
