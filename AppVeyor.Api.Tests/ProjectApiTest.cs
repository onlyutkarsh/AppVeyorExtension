using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppVeyor.Api.Tests
{
    [TestClass]
    public class ProjectApiTest
    {
        private AppVeyorService _appVeyorService;

        [TestInitialize]
        public void Init()
        {
            _appVeyorService = new AppVeyorService("YOUR_ACCOUNT_API_KEY_HERE");   
        }

        [TestMethod]
        [Ignore]
        public void GetProjects_Test()
        {
            var projects = _appVeyorService.ProjectApi.GetProjects();


        }

        [TestMethod]
        public async Task GetProjectsAsync_Test()
        {
            var projects = await _appVeyorService.ProjectApi.GetProjectsAsync();
            Assert.IsTrue(!projects.HasError && projects.Result != null && projects.Result.Count > 0);
        }

        [TestMethod]
        public async Task Get_Project_Last_Build_Test()
        {
            var projectSlug = "vsostatusinspector";
            var accountName = "onlyutkarsh";
            var buildInfo = await _appVeyorService.ProjectApi.GetLastBuild(accountName, projectSlug);
            Assert.IsTrue(buildInfo.Build != null && buildInfo.Project.Slug == projectSlug);
        }

        [TestMethod]
        public async Task Get_Project_By_Build_Version_Test()
        {
            var projectSlug = "vsostatusinspector";
            var accountName = "onlyutkarsh";
            var version = "1.0.1";
            var buildInfo = await _appVeyorService.ProjectApi.GetBuildByVersion(accountName, projectSlug, version);
            Assert.IsTrue(buildInfo.Build != null && buildInfo.Project.Slug == projectSlug);
        }

        [TestMethod]
        public async Task Get_Project_History()
        {
            var projectSlug = "vsostatusinspector";
            var accountName = "onlyutkarsh";
            var buildInfo = await _appVeyorService.ProjectApi.GetProjectHistory(accountName, projectSlug);
            Assert.IsTrue(buildInfo.Builds.Count == 10 && buildInfo.Project.Slug == projectSlug);
        }

        [TestMethod]
        public async Task Get_Project_History_With_BranchName()
        {
            var projectSlug = "vsostatusinspector";
            var accountName = "onlyutkarsh";
            var branchName = "master";
            var buildInfo = await _appVeyorService.ProjectApi.GetProjectHistory(accountName, projectSlug, branchName: branchName);
            Assert.IsTrue(buildInfo.Builds.Count == 10 && buildInfo.Project.Slug == projectSlug);
        }

        
        [TestMethod]
        public async Task Cancel_Build()
        {
            var projectSlug = "dropdownlinkbutton";
            var accountName = "onlyutkarsh";
            var branchName = "master";
            var build = await _appVeyorService.ProjectApi.CancelBuild(accountName, projectSlug, branchName);
            //Assert.IsTrue(build.BuildId != 0 && build.Status.First() == "queued");
        }
    }
}
