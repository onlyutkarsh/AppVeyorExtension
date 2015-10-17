using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace AppVeyor.UI.Services
{
    [Guid("52020DFD-D3FD-4D55-A5CF-91C227BFD10B")]
    [ComVisible(true)]
    public interface ICommandManagerService
    {
        /// <summary>
        /// Registers the command.
        /// </summary>
        /// <param name="command">The command.</param>
        void RegisterCommand(OleMenuCommand command);
        /// <summary>
        /// Unregister command.
        /// </summary>
        /// <param name="command">The command.</param>
        void UnRegisterCommand(OleMenuCommand command);
    }

    [Guid("6938D05D-7DD9-4F9D-99F6-FD5D3276FFAD")]
    public interface SCommandManagerService
    {
    }


}
