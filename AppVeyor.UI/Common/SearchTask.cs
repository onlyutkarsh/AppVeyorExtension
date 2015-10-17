using AppVeyor.UI.ToolWindows;
using AppVeyor.UI.ViewModel;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace AppVeyor.UI.Common
{
    internal class SearchTask : VsSearchTask
    {
        public SearchTask(uint dwCookie, IVsSearchQuery pSearchQuery, IVsSearchCallback pSearchCallback, AppVeyorWindow toolwindow)
            : base(dwCookie, pSearchQuery, pSearchCallback)
        
        {
            ;
        }

        protected override void OnStartSearch()
        {
            AppVeyorWindowViewModel.Instance.StopPollingForProjects();
            string searchString = this.SearchQuery.SearchString;
            AppVeyorWindowViewModel.Instance.Search(searchString);
            // Call the implementation of this method in the base class. 
            // This sets the task status to complete and reports task completion. 
            base.OnStartSearch();
        }
    }
}