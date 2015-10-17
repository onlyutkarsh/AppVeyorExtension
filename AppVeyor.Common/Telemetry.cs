using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace AppVeyor.Common
{
    public class Telemetry
    {
        private static readonly TelemetryClient _telemetryClient = new TelemetryClient();
        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static Telemetry()
        {
#if DEBUG
            TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = true;
#endif
            _telemetryClient.InstrumentationKey = "NA";
            _telemetryClient.Context.InstrumentationKey = "NA";
            _telemetryClient.Context.Session.Id = Guid.NewGuid().ToString();
            _telemetryClient.Context.User.AccountId = string.Format("{0}\\{1}", Environment.UserDomainName, Environment.UserName);
            _telemetryClient.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
            _telemetryClient.Context.Device.Language = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            _telemetryClient.Context.Properties["AppVeyor Extension Version"] = "1.0";

            TelemetryConfiguration.Active.DisableTelemetry = true;
        }

        private Telemetry()
        {
        }

        public static TelemetryClient Instance
        {
            get { return _telemetryClient; }
        }
    }
}
