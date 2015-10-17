using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using AppVeyor.Common.Extensions;

namespace AppVeyor.Commands.Base
{
    public abstract class DynamicCommand : OleMenuCommand
    {
        #region Fields
        private static IServiceProvider _serviceServiceProvider;
        private static AppVeyorPackage _appVeyorPackage;

        #endregion

        #region Properties
        /// <summary>
        /// The ServiceProvider
        /// </summary>
        protected static IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceServiceProvider;
            }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicCommand"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="onExecute">The on execute delegate.</param>
        /// <param name="id">The command id.</param>
        public DynamicCommand(IServiceProvider serviceProvider, EventHandler onExecute, CommandID id)
            : base(onExecute, id)
        {
            BeforeQueryStatus += new EventHandler(OnBeforeQueryStatus);
            _serviceServiceProvider = serviceProvider;
        }
        #endregion

        #region Protected Implementation
        /// <summary>
        /// Called when [before query status].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void OnBeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand command = sender as OleMenuCommand;

            command.Enabled = command.Supported = CanExecute(command);
        }

        /// <summary>
        /// Determines whether this instance can execute the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can execute the specified command; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool CanExecute(OleMenuCommand command)
        {
            return true;
        }
        #endregion

        protected static AppVeyorPackage AppVeyorPackage
        {
            get
            {
                if (_appVeyorPackage == null)
                {
                    _appVeyorPackage = ServiceProvider.GetService<AppVeyorPackage>();
                }

                return _appVeyorPackage;
            }
        }
    }
}
