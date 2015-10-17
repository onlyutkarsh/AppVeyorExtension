using System.Runtime.InteropServices;
using AppVeyor.UI.Options;

namespace AppVeyor.UI.Services
{
    [Guid("5E3E9E88-DA0D-4101-A3B5-657608480838")]
    [ComVisible(true)]
    public interface IEventManager
    {
        void ContextChanged(string newContext);
        //void OptionsChanged(OptionsChangedEventArgs optionsChangedEventArgs);
        AppVeyorOptions AppVeyorOptions { get; set; }
        void RefreshTriggerred();
    }

    [Guid("C285F675-EC35-4F44-B3D4-47B00B7B4FB2")]
    public class SEventManager
    {
        
    }
}