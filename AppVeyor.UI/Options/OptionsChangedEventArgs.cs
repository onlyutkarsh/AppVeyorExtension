namespace AppVeyor.UI.Options
{
    public class OptionsChangedEventArgs
    {
        //public bool OpenLinksInVisualStudio { get; set; }
        //public bool ConfirmActions { get; set; }

        public AppVeyorOptions AppVeyorOptions { get; set; }
    }
}