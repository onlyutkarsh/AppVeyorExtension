// PkgCmdID.cs
// MUST match PkgCmdID.h

namespace AppVeyor
{
    static class PkgCmdIDList
    {
        public static uint cmdidMyDropDownCombo = 0x1090;
        public static uint cmdidMyDropDownComboGetList = 0x1200;
        public const uint cmdidViewAppVeyor =        0x100;
        public const uint cmdidAppVeyor =    0x101;
        public const uint AppVeyorToolbar = 0x1000;

        public const uint cmdidProjectsAndBuilds = 0x1060;
        public const uint cmdidTeamsAndUsers = 0x1070;
        public const uint cmdidEnvironmentsAndDeployments = 0x1080;
    };
}