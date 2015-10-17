using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using AppVeyor.Common;
using AppVeyor.Common.Extensions;
using AppVeyor.UI.Options;
using AppVeyor.UI.Services;
using AppVeyor.UI.ToolWindows;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Constants = AppVeyor.UI.Common.Constants;

namespace AppVeyor
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(AppVeyorWindow))]
    [Guid(GuidList.guidAppVeyor_PackagePkgString)]
    [ProvideOptionPage(typeof(AppVeyorOptions), Constants.EXTENSION_NAME, "General", 0, 0, true)]
    [ProvideService(typeof(SEventManager))]
    [ProvideService(typeof(SCommandManagerService))]
    [ProvideBindingPath]
    public sealed class AppVeyorPackage : Package
    {
        private AppVeyorWindow _appVeyorToolWindow;

        public ToolWindowPane ToolWindow
        {
            get
            {
                if (_appVeyorToolWindow == null)
                {
                    Telemetry.Instance.TrackEvent("Find AppVeyor ToolWindow");
                    _appVeyorToolWindow = FindToolWindow(typeof(AppVeyorWindow), 0, true) as AppVeyorWindow;
                }

                return _appVeyorToolWindow;
            }
        }

        protected override void Initialize()
        {
            var dte = this.GetService<DTE>();
            var version = dte.Version;
            base.Initialize();
            var properties = new Dictionary<string, string>();
            properties["Visual Studio Version"] = string.Format("{0} {1}", version, dte.Edition) ;


            IServiceContainer serviceContainer = this;
            ServiceCreatorCallback creationCallback = CreateService;
            serviceContainer.AddService(typeof(SCommandManagerService), creationCallback, true);
            serviceContainer.AddService(typeof(SEventManager), creationCallback, true);

            var options = GetDialogPage(typeof (AppVeyorOptions)) as AppVeyorOptions;
            if (options != null)
            {
                var eventManager = this.GetService<SEventManager, IEventManager>();
                eventManager.AppVeyorOptions = options;
            }
            CommandSet commandSet = new CommandSet(this);
            commandSet.Initialize();

            Telemetry.Instance.TrackEvent("Extension initialized", properties);
        }

        private object CreateService(IServiceContainer container, Type serviceType)
        {
            if (container != this)
            {
                return null;
            }

            if (typeof(SCommandManagerService) == serviceType)
            {
                return new CommandManagerServiceService(this);
            }
            if (typeof(SEventManager) == serviceType)
            {
                return new EventManagerService(this);
            }

            return null;
        }
    }
}