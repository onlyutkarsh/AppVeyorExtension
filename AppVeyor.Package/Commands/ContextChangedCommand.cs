using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using AppVeyor.Commands.Base;

namespace AppVeyor.Commands
{
    [Guid("6cb5e405-3793-4a69-8acd-95c5f5f25df0")]
    public class ContextChangedCommand : DynamicCommand
    {
        public const uint cmdidMyDropDownCombo = 0x2910;
        public ContextChangedCommand(IServiceProvider serviceProvider)
            : base(
                    serviceProvider,
                    OnExecute,
                    new CommandID(
                        typeof(ContextChangedCommand).GUID,
                        (int)cmdidMyDropDownCombo))
        {
        }

        private static void OnExecute(object sender, EventArgs e)
        {
            
        }
    }
}
