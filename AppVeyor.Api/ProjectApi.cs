using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppVeyor.Common.Entities;
using CuttingEdge.Conditions;
using RestSharp;

namespace AppVeyor.Api
{
    public class ProjectApi : Resource
    {
        public ProjectApi(IRestClient client, string apiToken)
            : base(client, apiToken)
        {   

        }

        public List<Project> GetProjects()
        {
            var request = new RestRequest("projects", Method.GET);

            var projects = Execute<List<Project>>(request);
            return projects;
        }

        public async Task<AppVeyorServiceResponse<List<Project>>> GetProjectsAsync()
        {
            var request = new RestRequest("projects", Method.GET);
            var response = new AppVeyorServiceResponse<List<Project>>();
            try
            {
                var projects = await ExecuteAsync<List<Project>>(request);
                response.Result = projects;
            }
            catch (Exception exception)
            {
                response.Exception = exception;
                response.HasError = true;
            }

            return response;
        }

        public Task<BuildInfo> GetLastBuild(string accountName, string projectSlug)
        {
            var request = new RestRequest("projects/{accountName}/{projectSlug}", Method.GET);
            request.AddUrlSegment("accountName", accountName);
            request.AddUrlSegment("projectSlug", projectSlug);

            return ExecuteAsync<BuildInfo>(request);
        }

        public Task<BuildInfo> GetBuildByVersion(string accountName, string projectSlug, string buildVersion)
        {
            var request = new RestRequest("projects/{accountName}/{projectSlug}/build/{buildVersion}", Method.GET);
            request.AddUrlSegment("accountName", accountName);
            request.AddUrlSegment("projectSlug", projectSlug);
            request.AddUrlSegment("buildVersion", buildVersion);

            return ExecuteAsync<BuildInfo>(request);
        }

        public Task<ProjectHistory> GetProjectHistory(string accountName, string projectSlug, int noOfrecordsPerPage = 10, Guid buildId = default(Guid), string branchName = null)
        {
            var request = new RestRequest("projects/{accountName}/{projectSlug}/history", Method.GET);
            request.AddUrlSegment("accountName", accountName);
            request.AddUrlSegment("projectSlug", projectSlug);
            request.AddParameter("recordsNumber", noOfrecordsPerPage);
            if (buildId != Guid.Empty)
            {
                request.AddParameter("startBuildId", buildId);
            }
            if (!string.IsNullOrEmpty(branchName))
            {
                request.AddParameter("branch", branchName);
            }
            return ExecuteAsync<ProjectHistory>(request);
        }

        public virtual async Task<AppVeyorServiceResponse<Build>> StartBuild(string accountName, string projectSlug, string branchName)
        {
            var response = new AppVeyorServiceResponse<Build>();
            try
            {
                Condition.Requires(accountName, "accountName").IsNotNullOrEmpty();
                Condition.Requires(projectSlug, "projectSlug").IsNotNullOrEmpty();
                Condition.Requires(branchName, "branchName").IsNotNullOrEmpty();
                var request = new RestRequest("builds", Method.POST);
                request.AddJsonBody(new
                {
                    accountName,
                    projectSlug,
                    branch = branchName
                });
                var build = await ExecuteAsync<Build>(request);
                response.Result = build;
            }
            catch (Exception exception)
            {
                response.Exception = exception;
                response.HasError = true;
            }
            return response;
        }

        public async Task<AppVeyorServiceResponse<HttpStatusCode>> CancelBuild(string accountName, string projectSlug, string buildVersion)
        {
            var response = new AppVeyorServiceResponse<HttpStatusCode>();
            try
            {
                Condition.Requires(accountName, "accountName").IsNotNullOrEmpty();
                Condition.Requires(projectSlug, "projectSlug").IsNotNullOrEmpty();
                Condition.Requires(buildVersion, "buildVersion").IsNotNullOrEmpty();
                var request = new RestRequest("builds/{accountName}/{projectSlug}/{buildVersion}", Method.DELETE);
                request.AddUrlSegment("accountName", accountName);
                request.AddUrlSegment("projectSlug", projectSlug);
                request.AddUrlSegment("buildVersion", buildVersion);
                var stopResult = await ExecuteAsync<HttpStatusCode>(request);
                response.Result = stopResult;

            }
            catch (Exception exception)
            {
                response.Exception = exception;
                response.HasError = true;
            }
            return response;
           
        }
    }

}
