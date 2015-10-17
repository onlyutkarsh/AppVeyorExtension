using System;
using AppVeyor.Common;
using AppVeyor.UI.Options;
using AppVeyor.UI.ViewModel;

namespace AppVeyor.UI.Services
{
    public class EventManagerService: SEventManager, IEventManager
    {
        private IServiceProvider _serviceProvider;

        public EventManagerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            AppVeyorWindowViewModel.Instance.ServiceProvider = serviceProvider;
        }

        public void ContextChanged(string newContext)
        {
            AppVeyorWindowViewModel.Instance.ContextChanged(newContext);
        }

        public AppVeyorOptions AppVeyorOptions { get; set; }
        public void RefreshTriggerred()
        {
            Telemetry.Instance.TrackEvent("Force refresh");
            AppVeyorWindowViewModel.Instance.Initialize();
        }
    }
}
