using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace AppVeyor.UI.Options
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public partial class AppVeyorOptions : DialogPage
    {
        [Category("General")]
        [DisplayName(@"Prompt before build start")]
        [Description("If set to true, prompts for confirmation before starting the build.")]
        public bool ConfirmStart { get; set; }

        [Category("General")]
        [DisplayName(@"Prompt before build stop")]
        [Description("If set to true, prompts for confirmation before stopping the build.")]
        public bool ConfirmStop { get; set; }

        //[Category("General")]
        //[DisplayName(@"Open links in Visual Studio")]
        //[Description("If set to true, opens links in Visual Studio browser window. Otherwise, will attempt to open links in external browser.")]
        //public bool OpenLinksInVisualStudio { get; set; }

        [Category("API Token")]
        [DisplayName(@"API Token")]
        [Description("API token for connecting to AppVeyor. Token can be found on API token page under your AppVeyor account.")]
        public string ApiToken { get; set; }

        protected override void OnApply(PageApplyEventArgs e)
        {
            base.OnApply(e);
            if (OptionsChanged != null)
            {
                var optionsEventArg = new OptionsChangedEventArgs
                {
                    AppVeyorOptions = this
                };
                OptionsChanged(this, optionsEventArg);
            }
        }

        public event EventHandler<OptionsChangedEventArgs> OptionsChanged;
    }
}
