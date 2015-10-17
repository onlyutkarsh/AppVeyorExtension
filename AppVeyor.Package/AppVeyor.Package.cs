namespace AppVeyor
{
    using System;
    
    /// <summary>
    /// Helper class that exposes all GUIDs used across VS Package.
    /// </summary>
    internal sealed partial class GuidList
    {
        public const string guidAppVeyor_PackagePkgString = "a709c9b4-8b21-492f-aaa1-ed045db35043";
        public const string guidAppVeyor_PackageCmdSetString = "6cb5e405-3793-4a69-8acd-95c5f5f25df0";
        public const string guidShowAppVeyorWindowString = "6f6a1551-07cb-488c-835e-9c8a594e4c31";
        public const string guidShowProjectsTabString = "8a9536df-ec50-45ff-82dd-3c489480b27b";
        public const string guidShowUsersTabString = "5dc028a9-c7cf-494e-8d8b-c3729c4ed9ab";
        public const string guidShowEnvironmentsTabString = "855be23b-63f4-4212-8b6f-70afe1b309dc";
        public const string guidShowOptionsString = "eff56f04-9785-4b17-acee-00f8b8915308";
        public const string guidShowRefreshString = "6f3c0d89-2a18-48e3-9a55-041cb7ef7899";
        public const string guidImagesString = "e2abc2ed-f84b-4ce5-97b0-d393ba728b27";
        public static Guid guidAppVeyor_PackagePkg = new Guid(guidAppVeyor_PackagePkgString);
        public static Guid guidAppVeyor_PackageCmdSet = new Guid(guidAppVeyor_PackageCmdSetString);
        public static Guid guidShowAppVeyorWindow = new Guid(guidShowAppVeyorWindowString);
        public static Guid guidShowProjectsTab = new Guid(guidShowProjectsTabString);
        public static Guid guidShowUsersTab = new Guid(guidShowUsersTabString);
        public static Guid guidShowEnvironmentsTab = new Guid(guidShowEnvironmentsTabString);
        public static Guid guidShowOptions = new Guid(guidShowOptionsString);
        public static Guid guidShowRefresh = new Guid(guidShowRefreshString);
        public static Guid guidImages = new Guid(guidImagesString);
    }
    /// <summary>
    /// Helper class that encapsulates all CommandIDs uses across VS Package.
    /// </summary>
    internal sealed partial class PackageCommands
    {
        public const int AppVeyorToolbar = 0x1000;
        public const int AppVeyorToolbarGroup = 0x1050;
        public const int cmdidMyDropDownCombo = 0x1100;
        public const int cmdidMyDropDownComboGetList = 0x1101;
        public const int cmdidShowAppVeyorWindow = 0x0100;
        public const int appVeyorMenuGroup = 0x1020;
        public const int cmdidShowProjectsTab = 0x1060;
        public const int cmdidShowUsersTab = 0x1070;
        public const int cmdidShowEnvironmentsTab = 0x1080;
        public const int cmdidShowOptions = 0x1090;
        public const int cmdidShowRefresh = 0x1100;
        public const int projectsIcon = 0x0001;
        public const int environmentsIcon = 0x0002;
        public const int usersIcon = 0x0003;
        public const int appVeyorIcon = 0x0004;
        public const int optionsIcon = 0x0005;
        public const int refreshIcon = 0x0006;
    }
}
