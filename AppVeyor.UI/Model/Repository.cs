using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AppVeyor.Api;
using AppVeyor.Common.Entities;
using AppVeyor.Common.Exceptions;
using AppVeyor.Common.Extensions;

namespace AppVeyor.UI.Model
{
    public class Repository
    {
        private readonly AppVeyorService _appVeyorService;

        public Repository(string apiToken)
        {
            _appVeyorService = new AppVeyorService(apiToken);

        }

        public async Task<AppVeyorServiceResponse<List<Project>>> GetProjects()
        {
            var appVeyorServiceResponse = await _appVeyorService.ProjectApi.GetProjectsAsync();
            return appVeyorServiceResponse;
        }

        public async Task<AppVeyorServiceResponse<Build>> StartBuild(Project project)
        {
            var appVeyorServiceResponse = await _appVeyorService.ProjectApi.StartBuild(project.AccountName, project.Slug, project.RepositoryBranch);
            return appVeyorServiceResponse;
        }

        public async Task<AppVeyorServiceResponse<HttpStatusCode>> CancelBuild(Project project)
        {
            AppVeyorServiceResponse<HttpStatusCode> appVeyorServiceResponse;
            var buildVerionString = project.ToBuildVerionString();
            if (!string.IsNullOrEmpty(buildVerionString))
            {
                appVeyorServiceResponse = await _appVeyorService.ProjectApi.CancelBuild(project.AccountName, project.Slug, buildVerionString);
            }
            else
            {
                appVeyorServiceResponse = new AppVeyorServiceResponse<HttpStatusCode>();
                appVeyorServiceResponse.Exception = new AppVeyorException("Unable to parse version from project.");
                appVeyorServiceResponse.HasError = true;
            }
            return appVeyorServiceResponse;
        }

        public TimeSpan UpdatePollingInterval(TimeSpan currentPollDuration, List<Project> projects, bool isPollingCancelled)
        {
            var newPollDuration = currentPollDuration;

            if (projects.Any(p => p.Builds.Any(b => string.IsNullOrEmpty(b.Finished))))
                newPollDuration = TimeSpan.FromSeconds(5);

            if (projects.Any(p => p.Builds.Any(b => !string.IsNullOrEmpty(b.Finished))))
                newPollDuration = TimeSpan.FromSeconds(30);

            if (isPollingCancelled)
                newPollDuration = TimeSpan.FromSeconds(10);

            return newPollDuration;
        }

        //private async Task<List<Project>> GetProjects()
        //{
        //    // Get projects
        //    var model = new AppVeyorWindowModel();
        //    var projects = await model.GetProjects();
        //    //if (projects.Any())
        //    //{
        //    //    Projects = projects;
        //    //}

        //    return projects;
        //}
    }
}
