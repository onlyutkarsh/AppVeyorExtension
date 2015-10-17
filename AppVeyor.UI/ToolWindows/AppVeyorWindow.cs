using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using AppVeyor.UI.Common;
using AppVeyor.UI.ViewModel;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Constants = AppVeyor.UI.Common.Constants;

namespace AppVeyor.UI.ToolWindows
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    ///
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane, 
    /// usually implemented by the package implementer.
    ///
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its 
    /// implementation of the IVsUIElementPane interface.
    /// </summary>
    [Guid("1d4ae524-231a-41d4-96ed-3370ec2fe1f5")]
    public class AppVeyorWindow : ToolWindowPane, IVsWindowFrameNotify3
    {

        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public AppVeyorWindow() :
            base(null)
        {
            ToolBar = new CommandID(new Guid("6cb5e405-3793-4a69-8acd-95c5f5f25df0"), 0x1000);
            // Set the window title reading it from the resources.
            Caption = Constants.EXTENSION_NAME;
            // Set the image that will appear on the tab of the window frame
            // when docked with an other window
            // The resource ID correspond to the one defined in the resx file
            // while the Index is the offset in the bitmap strip. Each image in
            // the strip being 16x16.
            BitmapResourceID = 301;
            BitmapIndex = 1;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
            // the object returned by the Content property.
            Content = new AppVeyorWindowContent();
        }

        public override bool SearchEnabled
        {
            get { return true; }
        }

        public override void ProvideSearchSettings(IVsUIDataSource pSearchSettings)
        {
            Utilities.SetValue(pSearchSettings, SearchSettingsDataSource.SearchWatermarkProperty.Name, "Search");
        }

        public override IVsSearchTask CreateSearch(uint dwCookie, IVsSearchQuery pSearchQuery, IVsSearchCallback pSearchCallback)
        {
            if (pSearchQuery == null || pSearchCallback == null)
                return null;
            return new SearchTask(dwCookie, pSearchQuery, pSearchCallback, this);
        }

        public override void ClearSearch()
        {
            //clear search
            AppVeyorWindowViewModel.Instance.ClearSearch();
            AppVeyorWindowViewModel.Instance.StartPollingForProjects();
        }


        public int OnShow(int fShow)
        {
            AppVeyorWindowViewModel.Instance.Initialize();
            return VSConstants.S_OK;
        }

        public int OnMove(int x, int y, int w, int h)
        {
            return VSConstants.S_OK;
        }

        public int OnSize(int x, int y, int w, int h)
        {
            return VSConstants.S_OK;
        }

        public int OnDockableChange(int fDockable, int x, int y, int w, int h)
        {
            var dockable = fDockable != 0;
            if (!dockable)
            {

            }
            return VSConstants.S_OK;
        }

        public int OnClose(ref uint pgrfSaveOptions)
        {
            AppVeyorWindowViewModel.Instance.StopPollingForProjects();
            return VSConstants.S_OK;
        }
    }
}
